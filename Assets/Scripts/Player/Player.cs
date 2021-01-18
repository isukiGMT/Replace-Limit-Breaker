using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerSet
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        // <Movement>
        private float InputX, InputZ, gravity;
        public float Speed;
        bool isAttacked = false;
        private Camera cam;
        private CharacterController characterController;
        public float JumpForce;
        GameObject enemy;
        public bool isMove = false;
        public bool CanMove = true;
        public static GameObject Target;
        private Vector3 desiredMoveDirection;
        private bool FirstLock = true;
        private int EnemyIndex;
        public int TargetCode;

        [SerializeField] float rotationSpeed = 1f;
        [SerializeField] float allowRotation = 0.1f;
        public float movementSpeed = 1f;
        [SerializeField] float gravityMultipler;
        //<Animations>
        public Animator anim;
        private const string A_Idle = "Idle", A_Strafe = "Strafe", A_Run = "Run", A_Block = "Block", A_Roll = "Roll", A_Dead = "Dead", A_Attack = "Attack1",A_Attack2 = "Attack2";
        private string CurrentStates;
        // <Status>
        //<Status Float>
        public float CurrentHP = 0;
        public float CurrentEP = 0;
        public float MaximumHP = 0;
        public float MaximumEP = 0;
        public float Stamina = 0;
        public float MaxStamina = 0;
        public float BonusHP = 0;
        public float BonusEP = 0;
        public float BaseDamage = 25;
        public float Endurance = 10;
        public float DamageBonus = 0;
        private float BaseDef = 0;
        public float DefBonus = 0;
        public float BaseSpeed = 1;
        public float SpeedBonus = 0;
        public float BaseCriticalChance = 0;
        public float CriticalChanceBonus = 0;
        private float NoOfClick = 0;
        private float LastTimeAttack = 0;
        private float MaxAttackDelay = 1.4f;
        private float MaxClick = 2;
        public float StaminaRegenerateSpeed = 25;
        public float LastTimeLoseStamina;
        private float AttackSpeed = 0.75f;
        [HideInInspector] public float LV;
        [HideInInspector] public float EvolutionPoints;
        [HideInInspector] public float NextLV;
        //<Status Crowd Control>
        [HideInInspector] public enum AllCC { Stun, OutOfBlockPoint, Bleeding, Fire, Poison, invicible };
        private bool Dodge = false;
        [HideInInspector] public AllCC[] CurrentCC;
        //<Status Bool>
        private bool BeenHit = false;
        public bool Attacking = false;
        public bool CanChargeStamina = true;
        private bool Run = false;
        public bool _Block = false;
        public bool _Roll = false;
        public static bool LockTarget = false;
        //Enemies
        public GameObject ClosetEnemy;
        public List<Enemy> EnemiesInRange = new List<Enemy>();
        //Status String
        public string PlayerName;
        //Skill
        [Header("Skill")]
        [Space]
        [Header("Player Skill")]
        public Skill Skill1;
        public Skill Skill2;
        public Skill Skill3;
        public Skill Skill4;
        public Skill SkillCurrent;
        private int CurrentSkillNumber = 1;
        [Header("Weapons")]
        [Space]
        public GameObject WeaponCurrent;
        private bool IsShow = false;
        [HideInInspector] public bool _CanBackstab = false;
        private GameObject _BSTarget;
        //namespace
        PlayerSet.PlayerStats _PlayerStats = new PlayerSet.PlayerStats();
        #region Start
        void Start()
        {
            _PlayerStats = new PlayerSet.PlayerStats();
            _PlayerStats.AdmitStats(this);
            _PlayerStats.DefaultStats(this);
            characterController = GetComponent<CharacterController>();
            cam = Camera.main;
            anim = GetComponent<Animator>();
            PlayerName = "Itsuki";
        }
        #endregion
        #region Update
        void Update()
        {
            _PlayerStats.AdmitStats(this);
            StatesManager();
            if (CanMove == true)
            {
                JumpForce = GetComponent<jump>().VerticalVelocity;
                InputX = Input.GetAxis("Horizontal") * 2;
                InputZ = Input.GetAxis("Vertical") * 2;
                CheckIsMove();
                InputDecider();
                MovementManager();
            }
            Animations();
            Combat();
            FindCloset();
            ChangeSkill();
            LockingTarget();
        }
        #endregion
        #region Void
        #region Combat Manager
        void Combat()
        {
            Attack();
            Block();
            Roll();
        }
        #endregion
        #region States And Animations
        void StatesManager()
        {
            if (CurrentStates != A_Attack && CurrentStates != A_Attack2 && CurrentStates != A_Roll)
            {
                if (InputX == 0 && InputZ == 0)
                {
                    ChangeAnimationStates(A_Idle);
                }
                else if (Run == true)
                {
                    ChangeAnimationStates(A_Run);
                    movementSpeed = 5f;
                }
                else
                {
                    ChangeAnimationStates(A_Strafe);
                    movementSpeed = 1f;
                }
            }
        }
        void ChangeAnimationStates(string Animation_Name)
        {
            if (CurrentStates == Animation_Name) return;
            else
            {
                var Animation = Animator.StringToHash(Animation_Name);
                CurrentStates = Animation_Name;
                anim.Play(Animation);
                anim.CrossFade(Animation_Name, 0.1f, -1);
            }
        }
        void Animations()
        {
            if (CurrentHP == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                ChangeAnimationStates("Dead");
            }
            if (LockTarget == true)
            {
                if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKey(KeyCode.LeftControl))
                {
                    anim.SetFloat("InputX", InputX);
                    anim.SetFloat("InputZ", InputZ);
                }
            }
            else
            {
                anim.SetFloat("InputZ", 1);
            }
        }
        #endregion
        #region Stats
        public void ChargingStamina(int Value)
        {
            if (Value == 1)
            {
                CanChargeStamina = true;
            }
            if (Value == 0)
            {
                CanChargeStamina = false;
            }
        }
        public void TakeStamina(float Value)
        {
            Stamina -= Value;
            LastTimeLoseStamina = Time.time;
        }
        public void TakeDamage(float Damage)
        {
            if (Dodge == false)
            {
                if (CurrentHP - Damage > 0)
                {
                    CurrentHP -= Damage;
                }
                else
                {
                    CurrentHP = 0;
                }
            }
            else
            {
                Debug.Log(PlayerName + " Dodged");
            }
        }
        #endregion
        #region Movement
        void InputDecider()
        {
            Speed = new Vector2(InputX, InputZ).sqrMagnitude;
            if (Input.GetKey(KeyCode.LeftControl)) Run = true;
            else Run = false;
            if (Speed > allowRotation)
            {
                RotationManager();
            }
            else
            {
                desiredMoveDirection = Vector3.zero;
            }

        }



        void RotationManager()
        {
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            if (LockTarget == false && cam.GetComponent<Cinemachine.CinemachineBrain>().IsBlending == false)
            {
                desiredMoveDirection = forward * InputZ + right * InputX;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
            }
            else
            {
                desiredMoveDirection = forward * InputZ + right * InputX;
            }

        }

        void MovementManager()
        {

            Vector3 moveDirection = desiredMoveDirection * (movementSpeed * Time.deltaTime);
            moveDirection = new Vector3(moveDirection.x, JumpForce * Time.deltaTime, moveDirection.z);
            characterController.Move(moveDirection);

        }
        void CheckIsMove()
        {
            if (InputX != 0 || InputZ != 0)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }

        }
        public void ChangeMove(float Value)
        {
            if (Value == 0)
            {
                CanMove = false;
            }
            else
            {
                CanMove = true;
            }
        }
        void Block()
        {

            if (isAttacked == false && _Roll == false && Input.GetKey(KeyCode.LeftShift)) _Block = true;
            else _Block = false;
        }
        void Roll()
        {

            if (isAttacked == false && _Roll == false && Input.GetKeyDown(KeyCode.Space) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") && CurrentHP > 0 && CurrentStates != A_Attack && CurrentStates != A_Attack2)
            {
                if (ClosetEnemy != null)
                {
                    ChangeAnimationStates(A_Roll);
                }
                _Roll = true;
                Dodge = true;
            }
        }
        public void EndOfRoll()
        {
            _Roll = false;
            ChangeAnimationStates(A_Idle);
        }
        public void ExitDodge()
        {
            Dodge = false;
        }
        #endregion
        #region Basic Attack
        void Attack()
        {
            NoOfClick = Mathf.Clamp(NoOfClick, 0, MaxClick);
            if (Input.GetKeyDown(KeyCode.Mouse0) && _CanBackstab == false && _Roll == false)
            {
                _Roll = false;
                if (Stamina >= 25)
                {
                    NoOfClick++;
                    LastTimeAttack = Time.time;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && _CanBackstab == true)
            {
                transform.position = new Vector3(_BSTarget.transform.position.x, _BSTarget.transform.position.y, _BSTarget.transform.position.z - 1);
            }
            if (NoOfClick > 0 && Time.time - LastTimeAttack >= MaxAttackDelay)
            {
                ChangeAnimationStates(A_Idle);
                NoOfClick = 0;
            }
            if (NoOfClick == 1)
            {
                ChangeAnimationStates(A_Attack);
            }
            _WeaponManager();
        }
        public void ChangeAttack(int Value)
        {
            if (Value == 0)
            {
                Attacking = false;
                anim.speed = 1;
            }
            else
            {
                Attacking = true;
                anim.speed = AttackSpeed;
            }
        }
        public void EndOfAttack1()
        {
            if (NoOfClick == 2)
            {
                ChangeAnimationStates(A_Attack2);
            }
            else
            {
                ChangeAnimationStates(A_Idle);
                NoOfClick = 0;
            }
        }
        public void EndOfAttack2()
        {
            ChangeAnimationStates(A_Idle);
           NoOfClick = 0;
        }
        #endregion
        #region Skill System
        void ChangeSkill()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurrentSkillNumber = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurrentSkillNumber = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CurrentSkillNumber = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CurrentSkillNumber = 4;
            }
            if (CurrentSkillNumber == 1)
            {
                SkillCurrent = Skill1;
            }
            else if (CurrentSkillNumber == 2)
            {
                SkillCurrent = Skill2;
            }
            else if (CurrentSkillNumber == 3)
            {
                SkillCurrent = Skill3;
            }
            else
            {
                SkillCurrent = Skill4;
            }
            if (Input.GetKeyDown(KeyCode.C) && SkillCurrent.CanUse == true)
            {
                SkillCurrent.Use();
                SkillCurrent.SetLast();
                SkillCurrent.CanUse = false;
            }
            Cooldown();
        }
        void Cooldown()
        {
            if (SkillCurrent.CanUse == false)
            {
                if (Time.time - SkillCurrent.LastTimeUse >= SkillCurrent.Cooldown)
                {
                    SkillCurrent.CanUse = true;
                }
            }
            else
            {
                SkillCurrent.Default();
            }
        }
        #endregion
        #region Aim Enemy
        void FindCloset()
        {
            float distanceToClosetEnemy = Mathf.Infinity;
            GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject CurrentEnemy in AllEnemy)
            {
                float distanceToEnemy = (CurrentEnemy.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosetEnemy)
                {
                    distanceToClosetEnemy = distanceToEnemy;
                    ClosetEnemy = CurrentEnemy;
                }
            }
        }
        void LockingTarget()
        { 
            Enemy[] AllEnemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy CurrentEnemy in AllEnemies)
            {
                float DistanceToEnemy = Vector3.Distance(CurrentEnemy.transform.position, transform.position);
                if (DistanceToEnemy <= 10 && !EnemiesInRange.Contains(CurrentEnemy))
                {
                    EnemiesInRange.Add(CurrentEnemy);
                    EnemyIndex++;
                }
                else if (DistanceToEnemy > 10 && EnemiesInRange.Contains(CurrentEnemy))
                {
                    EnemiesInRange.Remove(CurrentEnemy);
                    EnemyIndex--;
                }
            }
            if (LockTarget == false)
            {
                if (Input.GetKeyDown(KeyCode.Q) && CurrentHP > 0 && EnemyIndex >= 1) LockTarget = true;
            }
            else
            {
                if (FirstLock && EnemyIndex >= 1)
                {
                    for (var i = 0; i <= EnemyIndex - 1; i++)
                    {
                        if (EnemiesInRange[i].gameObject == ClosetEnemy.gameObject)
                        {
                            FirstLock = false;
                            TargetCode = i;
                        }
                    }
                }
                else
                {
                    Target = EnemiesInRange[TargetCode].gameObject;
                    Vector3 Dir = new Vector3(Target.transform.position.x,transform.position.y,Target.transform.position.z);
                    transform.LookAt(Dir);
                    float Distance = Vector3.Distance(Target.transform.position,transform.position);
                    if (EnemyIndex > 1)
                    {
                        if (Input.GetKeyDown(KeyCode.Tab)) {
                            if (TargetCode == EnemyIndex - 1)
                            {
                                TargetCode = 0;
                            }
                            else
                            {
                                TargetCode++;
                            }
                        }
                    }
                    if (Distance > 10 || Target.GetComponent<Enemy>().EnemyHP <= 0 || Target == null) 
                    {
                        LockTarget = false;
                        EnemiesInRange.Clear();
                        EnemyIndex = 0;
                        Target = null;
                        FirstLock = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Q) && CurrentHP > 0 && Target != null)
                {
                    LockTarget = false;
                    EnemiesInRange.Clear();
                    EnemyIndex = 0;
                    Target = null;
                    FirstLock = true;
                }
            }
        }
        void _WeaponManager()
        {
            if (IsShow == false && NoOfClick >= 1)
            {
                IsShow = true;
            }
            if (IsShow == true && NoOfClick == 0)
            {
                StartCoroutine(TurnOffWeapon());
            }
            WeaponCurrent.SetActive(IsShow);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Backstab")) _CanBackstab = true;
            _BSTarget = other.gameObject;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Backstab")) _CanBackstab = false;
            _BSTarget = null;
        }
        private IEnumerator TurnOffWeapon()
        {
            yield return new WaitForSeconds(1);
            IsShow = false;
        }
    }
    #endregion
    #endregion
}

