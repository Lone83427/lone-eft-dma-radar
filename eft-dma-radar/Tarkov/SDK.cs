namespace SDK
{
	public readonly partial struct ClassNames
	{
		public readonly partial struct FixWildSpawnType
		{
			public const uint ClassName_ClassToken = 0x2002529; // MDToken
			public const string ClassName = @"\uECA8";
			public const string MethodName = @"SetUpSpawnInfo";
		}

		public readonly partial struct NetworkContainer
		{
			public const uint ClassName_ClassToken = 0x2000639; // MDToken
			public const string ClassName = @"\uE317";
		}

		public readonly partial struct AmmoTemplate
		{
			public const uint ClassName_ClassToken = 0x20028F2; // MDToken
			public const uint MethodName_MethodToken = 0x600F97D; // MDToken
			public const string ClassName = @"\uEE67";
			public const string MethodName = @"get_LoadUnloadModifier";
		}

		public readonly partial struct NoMalfunctions
		{
			public const uint ClassName_ClassToken = 0x20016A9; // MDToken
			public const uint GetMalfunctionState_MethodToken = 0x6009119; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
			public const string GetMalfunctionState = @"GetMalfunctionState";
		}

		public readonly partial struct GymHack
		{
			public const uint ClassName_ClassToken = 0x2002011; // MDToken
			public const uint MethodName_MethodToken = 0x600CA41; // MDToken
			public const string ClassName = @"EFT.Hideout.ShrinkingCircleQTE";
			public const string MethodName = @"\uE001";
		}

		public readonly partial struct OpticCameraManagerContainer
		{
			public const uint ClassName_ClassToken = 0x2002CEB; // MDToken
			public const string ClassName = @"\uF01E";
		}

		public readonly partial struct ScreenManager
		{
			public const uint ClassName_ClassToken = 0x20033D4; // MDToken
			public const string ClassName = @"\uF0E6";
		}

		public readonly partial struct FirearmController
		{
			public const uint ClassName_ClassToken = 0x20016A9; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
		}

		public readonly partial struct ProceduralWeaponAnimation
		{
			public const uint ClassName_ClassToken = 0x20023EB; // MDToken
			public const uint MethodName_MethodToken = 0x600DEDF; // MDToken
			public const string ClassName = @"EFT.Animations.ProceduralWeaponAnimation";
			public const string MethodName = @"get_ShotNeedsFovAdjustments";
		}
	}

	public readonly partial struct Offsets
	{
		public readonly partial struct TarkovApplication
		{
			public const uint MenuOperation = 0xF0; // -.\uE933
		}

		public readonly partial struct MenuOperation
		{
			public const uint AfkMonitor = 0x38; // -.\uE92B
		}

		public readonly partial struct AfkMonitor
		{
			public const uint Delay = 0x20; // Single
		}

		public readonly partial struct GameWorld
		{
			public const uint LootMaskObstruction = 0x14; // Int32
			public const uint Location = 0x90; // String
		}

		public readonly partial struct ClientLocalGameWorld
		{
			public const uint TransitController = 0x20; // -.\uE791
			public const uint ExfilController = 0x30; // -.\uE693
			public const uint BtrController = 0x50; // -.\uEFC1
			public const uint ClientShellingController = 0x80; // -.\uE6A0
			public const uint LocationId = 0x90; // String
			public const uint LootList = 0x118; // System.Collections.Generic.List<\uE2FB>
			public const uint RegisteredPlayers = 0x140; // System.Collections.Generic.List<IPlayer>
			public const uint BorderZones = 0x198; // EFT.Interactive.BorderZone[]
			public const uint MainPlayer = 0x1A8; // EFT.Player
			public const uint SynchronizableObjectLogicProcessor = 0x1D8; // -.\uEB2B
			public const uint Grenades = 0x200; // -.\uE3C2<Int32, Throwable>
		}

		public readonly partial struct TransitController
		{
			public const uint TransitPoints = 0x18; // System.Collections.Generic.Dictionary<Int32, TransitPoint>
		}

		public readonly partial struct ClientShellingController
		{
			public const uint ActiveClientProjectiles = 0x68; // System.Collections.Generic.Dictionary<Int32, ArtilleryProjectileClient>
		}

		public readonly partial struct ArtilleryProjectileClient
		{
			public const uint Position = 0x34; // UnityEngine.Vector3
			public const uint IsActive = 0x40; // Boolean
		}

		public readonly partial struct TransitPoint
		{
			public const uint parameters = 0x20; // -.\uE664.Location.TransitParameters
		}

		public readonly partial struct TransitParameters
		{
			public const uint name = 0x10; // String
			public const uint description = 0x18; // String
			public const uint location = 0x30; // String
		}

		public readonly partial struct SynchronizableObject
		{
			public const uint Type = 0x70; // System.Int32
		}

		public readonly partial struct SynchronizableObjectLogicProcessor
		{
			public const uint SynchronizableObjects = 0x18; // System.Collections.Generic.List<SynchronizableObject>
		}

		public readonly partial struct TripwireSynchronizableObject
		{
			public const uint GrenadeTemplateId = 0x110; // EFT.MongoID
			public const uint _tripwireState = 0x16C; // System.Int32
			public const uint FromPosition = 0x170; // UnityEngine.Vector3
			public const uint ToPosition = 0x17C; // UnityEngine.Vector3
		}

		public readonly partial struct MineDirectional
		{
			public const uint Mines = 0x8; // System.Collections.Generic.List<MineDirectional>
			public const uint MineData = 0x20; // -.MineDirectional.MineSettings
		}

		public readonly partial struct MineSettings
		{
			public const uint _maxExplosionDistance = 0x28; // Single
			public const uint _directionalDamageAngle = 0x64; // Single
		}

		public readonly partial struct BorderZone
		{
			public const uint Description = 0x38; // String
			public const uint _extents = 0x48; // UnityEngine.Vector3
		}

		public readonly partial struct LevelSettings
		{
			public const uint AmbientMode = 0x70; // System.Int32
			public const uint EquatorColor = 0x84; // UnityEngine.Color
			public const uint GroundColor = 0x94; // UnityEngine.Color
		}

		public readonly partial struct BtrController
		{
			public const uint BtrView = 0x30; // EFT.Vehicle.BTRView
		}

		public readonly partial struct BTRView
		{
			public const uint turret = 0x50; // EFT.Vehicle.BTRTurretView
			public const uint _targetPosition = 0xF8; // UnityEngine.Vector3
		}

		public readonly partial struct BTRTurretView
		{
			public const uint AttachedBot = 0x50; // System.ValueTuple<ObservedPlayerView, Boolean>
		}

		public readonly partial struct EFTHardSettings
		{
			public const uint DecelerationSpeed = 0x1A0; // Single
			public const uint LOOT_RAYCAST_DISTANCE = 0x230; // Single
			public const uint DOOR_RAYCAST_DISTANCE = 0x234; // Single
			public const uint STOP_AIMING_AT = 0x284; // Single
			public const uint MOUSE_LOOK_HORIZONTAL_LIMIT = 0x37C; // UnityEngine.Vector2
		}

		public readonly partial struct ExfilController
		{
			public const uint ExfiltrationPointArray = 0x28; // EFT.Interactive.ExfiltrationPoint[]
			public const uint ScavExfiltrationPointArray = 0x30; // EFT.Interactive.ScavExfiltrationPoint[]
		}

		public readonly partial struct Exfil
		{
			public const uint Settings = 0x78; // EFT.Interactive.ExitTriggerSettings
			public const uint EligibleEntryPoints = 0xA0; // System.String[]
			public const uint _status = 0xC8; // System.Byte
		}

		public readonly partial struct ScavExfil
		{
			public const uint EligibleIds = 0xE0; // System.Collections.Generic.List<String>
		}

		public readonly partial struct ExfilSettings
		{
			public const uint Name = 0x18; // String
		}

		public readonly partial struct GenericCollectionContainer
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Var>
		}

		public readonly partial struct Grenade
		{
			public const uint IsDestroyed = 0x5D; // Boolean
			public const uint WeaponSource = 0x80; // -.\uEEC9
		}

		public readonly partial struct Player
		{
			public const uint _characterController = 0x40; // -.ICharacterController
			public const uint MovementContext = 0x58; // EFT.MovementContext
			public const uint _playerBody = 0xC0; // EFT.PlayerBody
			public const uint ProceduralWeaponAnimation = 0x1E0; // EFT.Animations.ProceduralWeaponAnimation
			public const uint _animators = 0x3A0; // -.IAnimator[]
			public const uint Corpse = 0x3D8; // EFT.Interactive.Corpse
			public const uint Location = 0x5C8; // String
			public const uint InteractableObject = 0x5D8; // EFT.Interactive.InteractableObject
			public const uint Profile = 0x600; // EFT.Profile
			public const uint Physical = 0x610; // -.\uE383
			public const uint AIData = 0x620; // -.IAIData
			public const uint _healthController = 0x640; // EFT.HealthSystem.IHealthController
			public const uint _inventoryController = 0x658; // -.Player.PlayerInventoryController
			public const uint _handsController = 0x660; // -.Player.AbstractHandsController
			public const uint EnabledAnimators = 0x978; // System.Int32
			public const uint InteractionRayOriginOnStartOperation = 0x9E4; // UnityEngine.Vector3
			public const uint InteractionRayDirectionOnStartOperation = 0x9F0; // UnityEngine.Vector3
			public const uint IsYourPlayer = 0xA06; // Boolean
		}

		public readonly partial struct AIData
		{
			public const uint IsAI = 0xE8; // Boolean
		}

		public readonly partial struct ObservedPlayerView
		{
			public const uint GroupID = 0x20; // String
			public const uint NickName = 0x50; // String
			public const uint AccountId = 0x58; // String
			public const uint PlayerBody = 0x68; // EFT.PlayerBody
			public const uint ObservedPlayerController = 0x88; // -.\uEC95
			public const uint Voice = 0x98; // String
			public const uint Side = 0x100; // System.Int32
			public const uint IsAI = 0x111; // Boolean
			public const uint VisibleToCameraType = 0x114; // System.Int32
		}

		public readonly partial struct ObservedPlayerController
		{
			public const uint Player = 0x10; // EFT.NextObservedPlayer.ObservedPlayerView
			public static readonly uint[] MovementController = new uint[] { 0xC8, 0x10 }; // -.\uECB5, -.\uECB7
			public const uint HandsController = 0xD8; // -.\uEC9F
			public const uint InfoContainer = 0xE8; // -.\uECA8
			public const uint HealthController = 0xF0; // -.\uE430
			public const uint InventoryController = 0x118; // -.\uECAA
		}

		public readonly partial struct ObservedMovementController
		{
			public const uint Rotation = 0x88; // UnityEngine.Vector2
			public const uint Velocity = 0x10C; // UnityEngine.Vector3
		}

		public readonly partial struct ObservedHandsController
		{
			public const uint ItemInHands = 0x58; // EFT.InventoryLogic.Item
		}

		public readonly partial struct ObservedHealthController
		{
			public const uint Player = 0x10; // EFT.NextObservedPlayer.ObservedPlayerView
			public const uint PlayerCorpse = 0x18; // EFT.Interactive.ObservedCorpse
			public const uint HealthStatus = 0xD8; // System.Int32
		}

		public readonly partial struct SimpleCharacterController
		{
			public const uint _collisionMask = 0x60; // UnityEngine.LayerMask
			public const uint _speedLimit = 0x7C; // Single
			public const uint _sqrSpeedLimit = 0x80; // Single
			public const uint velocity = 0xEC; // UnityEngine.Vector3
		}

		public readonly partial struct InfoContainer
		{
			public const uint Side = 0x20; // System.Int32
		}

		public readonly partial struct PlayerSpawnInfo
		{
			public const uint Side = 0x28; // System.Int32
			public const uint WildSpawnType = 0x2C; // System.Int32
		}

		public readonly partial struct Physical
		{
			public const uint Stamina = 0x38; // -.\uE382
			public const uint HandsStamina = 0x40; // -.\uE382
			public const uint Oxygen = 0x48; // -.\uE382
			public const uint Overweight = 0x8C; // Single
			public const uint WalkOverweight = 0x90; // Single
			public const uint WalkSpeedLimit = 0x94; // Single
			public const uint Inertia = 0x98; // Single
			public const uint WalkOverweightLimits = 0xD8; // UnityEngine.Vector2
			public const uint BaseOverweightLimits = 0xE0; // UnityEngine.Vector2
			public const uint SprintOverweightLimits = 0xF4; // UnityEngine.Vector2
			public const uint SprintWeightFactor = 0x104; // Single
			public const uint SprintAcceleration = 0x114; // Single
			public const uint PreSprintAcceleration = 0x118; // Single
			public const uint IsOverweightA = 0x11C; // Boolean
			public const uint IsOverweightB = 0x11D; // Boolean
		}

		public readonly partial struct PhysicalValue
		{
			public const uint Current = 0x48; // Single
		}

		public readonly partial struct ProceduralWeaponAnimation
		{
			public const uint HandsContainer = 0x20; // EFT.Animations.PlayerSpring
			public const uint Breath = 0x30; // EFT.Animations.BreathEffector
			public const uint MotionReact = 0x40; // -.MotionEffector
			public const uint Shootingg = 0x50; // -.ShotEffector
			public const uint _optics = 0xC8; // System.Collections.Generic.List<SightNBone>
			public const uint Mask = 0x140; // System.Int32
			public const uint _isAiming = 0x1C5; // Boolean
			public const uint _aimingSpeed = 0x1E4; // Single
			public const uint _fovCompensatoryDistance = 0x1F8; // Single
			public const uint _compensatoryScale = 0x228; // Single
			public const uint _shotDirection = 0x22C; // UnityEngine.Vector3
			public const uint CameraSmoothOut = 0x268; // Single
			public const uint PositionZeroSum = 0x344; // UnityEngine.Vector3
			public const uint ShotNeedsFovAdjustments = 0x40F; // Boolean
		}

		public readonly partial struct SightNBone
		{
			public const uint Mod = 0x10; // EFT.InventoryLogic.SightComponent
		}

		public readonly partial struct MotionEffector
		{
			public const uint _mouseProcessors = 0x18; // -.\uE425[]
			public const uint _movementProcessors = 0x20; // -.\uE424[]
		}

		public readonly partial struct BreathEffector
		{
			public const uint Intensity = 0xA4; // Single
		}

		public readonly partial struct ShotEffector
		{
			public const uint NewShotRecoil = 0x18; // EFT.Animations.NewRecoil.NewRecoilShotEffect
		}

		public readonly partial struct NewShotRecoil
		{
			public const uint IntensitySeparateFactors = 0x94; // UnityEngine.Vector3
		}

		public readonly partial struct VisorEffect
		{
			public const uint Intensity = 0xC8; // Single
		}

		public readonly partial struct Profile
		{
			public const uint Id = 0x10; // String
			public const uint AccountId = 0x18; // String
			public const uint Info = 0x40; // -.\uE8D2
			public const uint Skills = 0x70; // EFT.SkillManager
			public const uint TaskConditionCounters = 0x80; // System.Collections.Generic.Dictionary<MongoID, \uF143>
			public const uint QuestsData = 0x88; // System.Collections.Generic.List<\uF15F>
			public const uint WishlistManager = 0xC8; // -.\uE800
			public const uint Stats = 0xF8; // -.\uE399
		}

		public readonly partial struct WishlistManager
		{
			public const uint Items = 0x28; // System.Collections.Generic.Dictionary<MongoID, Int32>
		}

		public readonly partial struct PlayerInfo
		{
			public const uint Nickname = 0x10; // String
			public const uint EntryPoint = 0x18; // String
			public const uint GroupId = 0x28; // String
			public const uint Settings = 0x50; // -.\uE8AE
			public const uint Side = 0x90; // [HUMAN] Int32
			public const uint RegistrationDate = 0x94; // Int32
			public const uint MemberCategory = 0xA0; // System.Int32
			public const uint Experience = 0xA4; // Int32
		}

		public readonly partial struct PlayerInfoSettings
		{
			public const uint Role = 0x10; // System.Int32
		}

		public readonly partial struct SkillManager
		{
			public const uint StrengthBuffJumpHeightInc = 0x60; // -.SkillManager.\uE004
			public const uint StrengthBuffThrowDistanceInc = 0x70; // -.SkillManager.\uE004
			public const uint MagDrillsLoadSpeed = 0x180; // -.SkillManager.\uE004
			public const uint MagDrillsUnloadSpeed = 0x188; // -.SkillManager.\uE004
		}

		public readonly partial struct SkillValueContainer
		{
			public const uint Value = 0x30; // Single
		}

		public readonly partial struct QuestData
		{
			public const uint Id = 0x10; // String
			public const uint CompletedConditions = 0x20; // System.Collections.Generic.HashSet<MongoID>
			public const uint Template = 0x28; // -.\uF160
			public const uint Status = 0x34; // System.Int32
		}

		public readonly partial struct QuestTemplate
		{
			public const uint Conditions = 0x40; // EFT.Quests.ConditionsDict
			public const uint Name = 0x50; // String
		}

		public readonly partial struct QuestConditionsContainer
		{
			public const uint ConditionsList = 0x50; // System.Collections.Generic.List<Var>
		}

		public readonly partial struct QuestCondition
		{
			public const uint id = 0x10; // EFT.MongoID
		}

		public readonly partial struct QuestConditionItem
		{
			public const uint value = 0x58; // Single
		}

		public readonly partial struct QuestConditionFindItem
		{
			public const uint target = 0x70; // System.String[]
		}

		public readonly partial struct QuestConditionCounterCreator
		{
			public const uint Conditions = 0x78; // -.\uF140
		}

		public readonly partial struct QuestConditionVisitPlace
		{
			public const uint target = 0x70; // String
		}

		public readonly partial struct QuestConditionPlaceBeacon
		{
			public const uint zoneId = 0x78; // String
			public const uint plantTime = 0x80; // Single
		}

		public readonly partial struct QuestConditionCounterTemplate
		{
			public const uint Conditions = 0x10; // -.\uF140
		}

		public readonly partial struct ItemHandsController
		{
			public const uint Item = 0x68; // EFT.InventoryLogic.Item
		}

		public readonly partial struct FirearmController
		{
			public const uint Fireport = 0xC8; // EFT.BifacialTransform
			public const uint TotalCenterOfImpact = 0x198; // Single
		}

		public readonly partial struct ClientFirearmController
		{
			public const uint WeaponLn = 0x184; // Single
			public const uint ShotIndex = 0x3F0; // SByte
		}

		public readonly partial struct MovementContext
		{
			public const uint Player = 0x10; // EFT.Player
			public const uint CurrentState = 0xE0; // EFT.BaseMovementState
			public const uint _states = 0x1E0; // System.Collections.Generic.Dictionary<Byte, BaseMovementState>
			public const uint _movementStates = 0x200; // -.IPlayerStateContainerBehaviour[]
			public const uint _tilt = 0x268; // Single
			public const uint _rotation = 0x27C; // UnityEngine.Vector2
			public const uint _physicalCondition = 0x300; // System.Int32
			public const uint _speedLimitIsDirty = 0x305; // Boolean
			public const uint StateSpeedLimit = 0x308; // Single
			public const uint StateSprintSpeedLimit = 0x30C; // Single
			public const uint _lookDirection = 0x420; // UnityEngine.Vector3
			public const uint WalkInertia = 0x4A0; // Single
			public const uint SprintBrakeInertia = 0x4A4; // Single
		}

		public readonly partial struct MovementState
		{
			public const uint Name = 0x21; // System.Byte
			public const uint AnimatorStateHash = 0x24; // Int32
			public const uint StickToGround = 0x5C; // Boolean
			public const uint PlantTime = 0x60; // Single
		}

		public readonly partial struct PlayerStateContainer
		{
			public const uint Name = 0x39; // System.Byte
			public const uint StateFullNameHash = 0x50; // Int32
		}

		public readonly partial struct InventoryController
		{
			public const uint Inventory = 0x118; // EFT.InventoryLogic.Inventory
		}

		public readonly partial struct Inventory
		{
			public const uint Equipment = 0x10; // EFT.InventoryLogic.InventoryEquipment
			public const uint QuestRaidItems = 0x20; // -.\uEF44
			public const uint QuestStashItems = 0x28; // -.\uEF44
		}

		public readonly partial struct Equipment
		{
			public const uint Grids = 0x78; // -.\uEDC3[]
			public const uint Slots = 0x80; // EFT.InventoryLogic.Slot[]
		}

		public readonly partial struct Grids
		{
			public const uint ContainedItems = 0x30; // -.\uEDC5
		}

		public readonly partial struct GridContainedItems
		{
			public const uint Items = 0x18; // System.Collections.Generic.List<Item>
		}

		public readonly partial struct Slot
		{
			public const uint ContainedItem = 0x38; // EFT.InventoryLogic.Item
			public const uint ID = 0x48; // String
			public const uint Required = 0x60; // Boolean
		}

		public readonly partial struct InteractiveLootItem
		{
			public const uint Item = 0xB8; // EFT.InventoryLogic.Item
		}

		public readonly partial struct InteractiveCorpse
		{
			public const uint PlayerBody = 0x130; // EFT.PlayerBody
		}

		public readonly partial struct DizSkinningSkeleton
		{
			public const uint _values = 0x30; // System.Collections.Generic.List<Transform>
		}

		public readonly partial struct LootableContainer
		{
			public const uint InteractingPlayer = 0xC0; // EFT.IPlayer
			public const uint ItemOwner = 0x130; // -.\uEEFC
			public const uint Template = 0x138; // String
		}

		public readonly partial struct LootableContainerItemOwner
		{
			public const uint RootItem = 0xB8; // EFT.InventoryLogic.Item
		}

		public readonly partial struct LootItem
		{
			public const uint Template = 0x40; // EFT.InventoryLogic.ItemTemplate
			public const uint StackObjectsCount = 0x64; // Int32
			public const uint Version = 0x68; // Int32
			public const uint SpawnedInSession = 0x6C; // Boolean
		}

		public readonly partial struct LootItemMod
		{
			public const uint Grids = 0x78; // -.\uEDC3[]
			public const uint Slots = 0x80; // EFT.InventoryLogic.Slot[]
		}

		public readonly partial struct LootItemWeapon
		{
			public const uint FireMode = 0xA0; // EFT.InventoryLogic.FireModeComponent
			public const uint Chambers = 0xB0; // EFT.InventoryLogic.Slot[]
			public const uint _magSlotCache = 0xC8; // EFT.InventoryLogic.Slot
		}

		public readonly partial struct FireModeComponent
		{
			public const uint FireMode = 0x28; // System.Byte
		}

		public readonly partial struct LootItemMagazine
		{
			public const uint Cartridges = 0xA0; // EFT.InventoryLogic.StackSlot
			public const uint LoadUnloadModifier = 0x19C; // Single
		}

		public readonly partial struct MagazineClass
		{
			public const uint StackObjectsCount = 0x64; // Int32
		}

		public readonly partial struct StackSlot
		{
			public const uint _items = 0x10; // System.Collections.Generic.List<Item>
			public const uint MaxCount = 0x38; // Int32
		}

		public readonly partial struct ItemTemplate
		{
			public const uint ShortName = 0x18; // String
			public const uint _id = 0x50; // EFT.MongoID
			public const uint Weight = 0xB0; // Single
			public const uint QuestItem = 0xBC; // Boolean
		}

		public readonly partial struct ModTemplate
		{
			public const uint Velocity = 0x168; // Single
		}

		public readonly partial struct AmmoTemplate
		{
			public const uint InitialSpeed = 0x1BC; // Single
			public const uint BallisticCoeficient = 0x1D0; // Single
			public const uint BulletMassGram = 0x258; // Single
			public const uint BulletDiameterMilimeters = 0x25C; // Single
		}

		public readonly partial struct WeaponTemplate
		{
			public const uint Velocity = 0x254; // Single
		}

		public readonly partial struct PlayerBody
		{
			public const uint SkeletonRootJoint = 0x30; // Diz.Skinning.Skeleton
			public const uint BodySkins = 0x48; // System.Collections.Generic.Dictionary<Int32, LoddedSkin>
			public const uint _bodyRenderers = 0x58; // -.\uE42F[]
			public const uint SlotViews = 0x70; // -.\uE3C2<Int32, \uE001>
		}

		public readonly partial struct PlayerBodySubclass
		{
			public const uint Dresses = 0x40; // EFT.Visual.Dress[]
		}

		public readonly partial struct Dress
		{
			public const uint Renderers = 0x30; // UnityEngine.Renderer[]
		}

		public readonly partial struct Skeleton
		{
			public const uint _values = 0x30; // System.Collections.Generic.List<Transform>
		}

		public readonly partial struct LoddedSkin
		{
			public const uint _lods = 0x20; // Diz.Skinning.AbstractSkin[]
		}

		public readonly partial struct Skin
		{
			public const uint _skinnedMeshRenderer = 0x28; // UnityEngine.SkinnedMeshRenderer
		}

		public readonly partial struct TorsoSkin
		{
			public const uint _skin = 0x28; // Diz.Skinning.Skin
		}

		public readonly partial struct SlotViewsContainer
		{
			public const uint Dict = 0x10; // System.Collections.Generic.Dictionary<Var, Var>
		}

		public readonly partial struct WeatherController
		{
			public const uint WeatherDebug = 0x68; // EFT.Weather.WeatherDebug
		}

		public readonly partial struct WeatherDebug
		{
			public const uint isEnabled = 0x18; // Boolean
			public const uint WindMagnitude = 0x1C; // Single
			public const uint CloudDensity = 0x2C; // Single
			public const uint Fog = 0x30; // Single
			public const uint Rain = 0x34; // Single
			public const uint LightningThunderProbability = 0x38; // Single
		}

		public readonly partial struct TOD_Scattering
		{
			public const uint sky = 0x20; // -.TOD_Sky
		}

		public readonly partial struct TOD_Sky
		{
			public const uint Cycle = 0x20; // -.TOD_CycleParameters
			public const uint TOD_Components = 0x80; // -.TOD_Components
		}

		public readonly partial struct TOD_CycleParameters
		{
			public const uint Hour = 0x10; // Single
		}

		public readonly partial struct TOD_Components
		{
			public const uint TOD_Time = 0x118; // -.TOD_Time
		}

		public readonly partial struct TOD_Time
		{
			public const uint LockCurrentTime = 0x70; // Boolean
		}

		public readonly partial struct NetworkContainer
		{
			public const uint NextRequestIndex = 0x8; // Int64
			public const uint PhpSessionId = 0x30; // String
			public const uint AppVersion = 0x38; // String
		}

		public readonly partial struct ScreenManager
		{
			public const uint Instance = 0x0; // -.\uF0E6
			public const uint CurrentScreenController = 0x28; // -.\uF0E8<Var>
		}

		public readonly partial struct CurrentScreenController
		{
			public const uint Generic = 0x20; // Var
		}

		public readonly partial struct OpticCameraManagerContainer
		{
			public const uint Instance = 0x0; // -.\uF01E
			public const uint OpticCameraManager = 0x10; // -.\uF01F
			public const uint FPSCamera = 0x60; // UnityEngine.Camera
		}

		public readonly partial struct OpticCameraManager
		{
			public const uint Camera = 0x68; // UnityEngine.Camera
			public const uint CurrentOpticSight = 0x70; // EFT.CameraControl.OpticSight
		}

		public readonly partial struct OpticSight
		{
			public const uint LensRenderer = 0x20; // UnityEngine.Renderer
		}

		public readonly partial struct SightComponent
		{
			public const uint _template = 0x20; // -.\uEDBB
			public const uint ScopesSelectedModes = 0x30; // System.Int32[]
			public const uint SelectedScope = 0x38; // Int32
			public const uint ScopeZoomValue = 0x3C; // Single
		}

		public readonly partial struct SightInterface
		{
			public const uint Zooms = 0x190; // System.Single[]
		}
	}

	public readonly partial struct Enums
	{
		public enum EPlayerState
		{
			None = 0,
			Idle = 1,
			ProneIdle = 2,
			ProneMove = 3,
			Run = 4,
			Sprint = 5,
			Jump = 6,
			FallDown = 7,
			Transition = 8,
			BreachDoor = 9,
			Loot = 10,
			Pickup = 11,
			Open = 12,
			Close = 13,
			Unlock = 14,
			Sidestep = 15,
			DoorInteraction = 16,
			Approach = 17,
			Prone2Stand = 18,
			Transit2Prone = 19,
			Plant = 20,
			Stationary = 21,
			Roll = 22,
			JumpLanding = 23,
			ClimbOver = 24,
			ClimbUp = 25,
			VaultingFallDown = 26,
			VaultingLanding = 27,
			BlindFire = 28,
			IdleWeaponMounting = 29,
			IdleZombieState = 30,
			MoveZombieState = 31,
			TurnZombieState = 32,
			StartMoveZombieState = 33,
			EndMoveZombieState = 34,
			DoorInteractionZombieState = 35,
		}

		[Flags]
		public enum EMemberCategory
		{
			Default = 0,
			Developer = 1,
			UniqueId = 2,
			Trader = 4,
			Group = 8,
			System = 16,
			ChatModerator = 32,
			ChatModeratorWithPermanentBan = 64,
			UnitTest = 128,
			Sherpa = 256,
			Emissary = 512,
			Unheard = 1024,
		}

		public enum WildSpawnType
		{
			marksman = 0,
			assault = 1,
			bossTest = 2,
			bossBully = 3,
			followerTest = 4,
			followerBully = 5,
			bossKilla = 6,
			bossKojaniy = 7,
			followerKojaniy = 8,
			pmcBot = 9,
			cursedAssault = 10,
			bossGluhar = 11,
			followerGluharAssault = 12,
			followerGluharSecurity = 13,
			followerGluharScout = 14,
			followerGluharSnipe = 15,
			followerSanitar = 16,
			bossSanitar = 17,
			test = 18,
			assaultGroup = 19,
			sectantWarrior = 20,
			sectantPriest = 21,
			bossTagilla = 22,
			followerTagilla = 23,
			exUsec = 24,
			gifter = 25,
			bossKnight = 26,
			followerBigPipe = 27,
			followerBirdEye = 28,
			bossZryachiy = 29,
			followerZryachiy = 30,
			bossBoar = 32,
			followerBoar = 33,
			arenaFighter = 34,
			arenaFighterEvent = 35,
			bossBoarSniper = 36,
			crazyAssaultEvent = 37,
			peacefullZryachiyEvent = 38,
			sectactPriestEvent = 39,
			ravangeZryachiyEvent = 40,
			followerBoarClose1 = 41,
			followerBoarClose2 = 42,
			bossKolontay = 43,
			followerKolontayAssault = 44,
			followerKolontaySecurity = 45,
			shooterBTR = 46,
			bossPartisan = 47,
			spiritWinter = 48,
			spiritSpring = 49,
			peacemaker = 50,
			pmcBEAR = 51,
			pmcUSEC = 52,
			skier = 53,
			sectantPredvestnik = 57,
			sectantPrizrak = 58,
			sectantOni = 59,
			infectedAssault = 60,
			infectedPmc = 61,
			infectedCivil = 62,
			infectedLaborant = 63,
			infectedTagilla = 64,
		}

		public enum EExfiltrationStatus
		{
			NotPresent = 1,
			UncompleteRequirements = 2,
			Countdown = 3,
			RegularMode = 4,
			Pending = 5,
			AwaitsManualActivation = 6,
			Hidden = 7,
		}

		public enum EMalfunctionState
		{
			None = 0,
			Misfire = 1,
			Jam = 2,
			HardSlide = 3,
			SoftSlide = 4,
			Feed = 5,
		}

		[Flags]
		public enum EPhysicalCondition
		{
			None = 0,
			OnPainkillers = 1,
			LeftLegDamaged = 2,
			RightLegDamaged = 4,
			ProneDisabled = 8,
			LeftArmDamaged = 16,
			RightArmDamaged = 32,
			Tremor = 64,
			UsingMeds = 128,
			HealingLegs = 256,
			JumpDisabled = 512,
			SprintDisabled = 1024,
			ProneMovementDisabled = 2048,
			Panic = 4096,
		}

		[Flags]
		public enum EProceduralAnimationMask
		{
			Breathing = 1,
			Walking = 2,
			MotionReaction = 4,
			ForceReaction = 8,
			Shooting = 16,
			DrawDown = 32,
			Aiming = 64,
			HandShake = 128,
		}

		public enum EquipmentSlot
		{
			FirstPrimaryWeapon = 0,
			SecondPrimaryWeapon = 1,
			Holster = 2,
			Scabbard = 3,
			Backpack = 4,
			SecuredContainer = 5,
			TacticalVest = 6,
			ArmorVest = 7,
			Pockets = 8,
			Eyewear = 9,
			FaceCover = 10,
			Headwear = 11,
			Earpiece = 12,
			Dogtag = 13,
			ArmBand = 14,
		}

		public enum EFireMode
		{
			fullauto = 0,
			single = 1,
			doublet = 2,
			burst = 3,
			doubleaction = 4,
			semiauto = 5,
			grenadeThrowing = 6,
			greanadePlanting = 7,
		}

		public enum SynchronizableObjectType
		{
			AirDrop = 0,
			AirPlane = 1,
			Tripwire = 2,
		}

		public enum ETripwireState
		{
			None = 0,
			Wait = 1,
			Active = 2,
			Exploding = 3,
			Exploded = 4,
			Inert = 5,
		}

		public enum EQuestStatus
		{
			Locked = 0,
			AvailableForStart = 1,
			Started = 2,
			AvailableForFinish = 3,
			Success = 4,
			Fail = 5,
			FailRestartable = 6,
			MarkedAsFailed = 7,
			Expired = 8,
			AvailableAfter = 9,
		}
	}
}
