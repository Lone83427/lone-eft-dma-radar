namespace SDK
{
	public readonly partial struct ClassNames
	{
		public readonly partial struct NoMalfunctions
		{
			public const uint ClassName_ClassToken = 0x2001B84; // MDToken
			public const uint GetMalfunctionState_MethodToken = 0x600AA62; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
			public const string GetMalfunctionState = @"GetMalfunctionState";
		}

		public readonly partial struct FirearmController
		{
			public const uint ClassName_ClassToken = 0x2001B84; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
		}

		public readonly partial struct OpticCameraManagerContainer
		{
			public const uint ClassName_ClassToken = 0x20034EB; // MDToken
			public const string ClassName = @"\uF208";
		}

		public readonly partial struct ProceduralWeaponAnimation
		{
			public const uint ClassName_ClassToken = 0x20028C6; // MDToken
			public const uint MethodName_MethodToken = 0x600F8ED; // MDToken
			public const string ClassName = @"EFT.Animations.ProceduralWeaponAnimation";
			public const string MethodName = @"get_ShotNeedsFovAdjustments";
		}
	}

	public readonly partial struct Offsets
	{
		public readonly partial struct TarkovApplication
		{
			public const uint GameOperationSubclass = 0xF0; // -.\uEA68
		}

		public readonly partial struct GameWorld
		{
			public const uint Location = 0x90; // String
		}

		public readonly partial struct ClientLocalGameWorld
		{
			public const uint LocationId = 0x90; // String
			public const uint RegisteredPlayers = 0x140; // System.Collections.Generic.List<IPlayer>
			public const uint MainPlayer = 0x1B0; // EFT.Player
			public const uint Grenades = 0x210; // -.\uE3E0<Int32, Throwable>
			public const uint IsInRaid = 0x290; // [HUMAN] Bool
		}

		public readonly partial struct Grenade
		{
			public const uint IsDestroyed = 0x5D; // Boolean
			public const uint WeaponSource = 0x80; // -.\uF10E
		}

		public readonly partial struct Player
		{
			public const uint MovementContext = 0x58; // EFT.MovementContext
			public const uint _playerBody = 0xC0; // EFT.PlayerBody
			public const uint ProceduralWeaponAnimation = 0x1E8; // EFT.Animations.ProceduralWeaponAnimation
			public const uint Corpse = 0x3E8; // EFT.Interactive.Corpse
			public const uint Profile = 0x620; // EFT.Profile
			public const uint _inventoryController = 0x680; // -.Player.PlayerInventoryController
			public const uint _handsController = 0x688; // -.Player.AbstractHandsController
		}

		public readonly partial struct ObservedPlayerView
		{
			public const uint NickName = 0x50; // String
			public const uint AccountId = 0x58; // String
			public const uint PlayerBody = 0x68; // EFT.PlayerBody
			public const uint ObservedPlayerController = 0x88; // -.\uEDC9
			public const uint Side = 0x108; // System.Int32
			public const uint IsAI = 0x119; // Boolean
			public const uint VisibleToCameraType = 0x11C; // System.Int32
		}

		public readonly partial struct ObservedPlayerController
		{
			public static readonly uint[] MovementController = new uint[] { 0xF0, 0x10 }; // -.\uEDEC, -.\uEDEE
			public const uint HandsController = 0x100; // -.\uEDD7
			public const uint HealthController = 0x118; // -.\uE451
			public const uint InventoryController = 0x140; // -.\uEDC4
		}

		public readonly partial struct ObservedMovementController
		{
			public const uint Rotation = 0x88; // UnityEngine.Vector2
			public const uint Velocity = 0x120; // UnityEngine.Vector3
		}

		public readonly partial struct ObservedHandsController
		{
			public const uint ItemInHands = 0x58; // EFT.InventoryLogic.Item
		}

		public readonly partial struct ObservedHealthController
		{
			public const uint PlayerCorpse = 0x18; // EFT.Interactive.ObservedCorpse
			public const uint HealthStatus = 0xE0; // System.Int32
		}

		public readonly partial struct ProceduralWeaponAnimation
		{
			public const uint HandsContainer = 0x20; // EFT.Animations.PlayerSpring
			public const uint Breath = 0x30; // EFT.Animations.BreathEffector
			public const uint MotionReact = 0x40; // -.MotionEffector
			public const uint Shootingg = 0x50; // -.ShotEffector
			public const uint _optics = 0xC8; // System.Collections.Generic.List<SightNBone>
			public const uint Mask = 0x158; // System.Int32
			public const uint _isAiming = 0x1DD; // Boolean
			public const uint ShotNeedsFovAdjustments = 0x427; // Boolean
		}

		public readonly partial struct SightNBone
		{
			public const uint Mod = 0x10; // EFT.InventoryLogic.SightComponent
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
			public const uint Info = 0x40; // -.\uE8A6
		}

		public readonly partial struct PlayerInfo
		{
			public const uint Nickname = 0x20; // String
			public const uint Settings = 0x60; // -.\uE9E6
			public const uint Side = 0xA8; // [HUMAN] Int32
			public const uint RegistrationDate = 0xAC; // Int32
			public const uint MemberCategory = 0xB8; // System.Int32
			public const uint Experience = 0xBC; // Int32
		}

		public readonly partial struct PlayerInfoSettings
		{
			public const uint Role = 0x10; // System.Int32
		}

		public readonly partial struct ItemHandsController
		{
			public const uint Item = 0x68; // EFT.InventoryLogic.Item
		}

		public readonly partial struct FirearmController
		{
			public const uint Fireport = 0xD0; // EFT.BifacialTransform
			public const uint TotalCenterOfImpact = 0x1A0; // Single
		}

		public readonly partial struct ClientFirearmController
		{
			public const uint ShotIndex = 0x408; // SByte
		}

		public readonly partial struct MovementContext
		{
			public const uint _rotation = 0x27C; // UnityEngine.Vector2
		}

		public readonly partial struct InventoryController
		{
			public const uint Inventory = 0x130; // EFT.InventoryLogic.Inventory
		}

		public readonly partial struct Inventory
		{
			public const uint Equipment = 0x10; // EFT.InventoryLogic.InventoryEquipment
		}

		public readonly partial struct Equipment
		{
			public const uint Grids = 0x90; // -.\uF002[]
			public const uint Slots = 0x98; // EFT.InventoryLogic.Slot[]
		}

		public readonly partial struct Slot
		{
			public const uint ContainedItem = 0x48; // EFT.InventoryLogic.Item
			public const uint ID = 0x58; // String
		}

		public readonly partial struct InteractiveLootItem
		{
			public const uint Item = 0xB8; // EFT.InventoryLogic.Item
		}

		public readonly partial struct InteractiveCorpse
		{
			public const uint PlayerBody = 0x138; // EFT.PlayerBody
		}

		public readonly partial struct DizSkinningSkeleton
		{
			public const uint _values = 0x30; // System.Collections.Generic.List<Transform>
		}

		public readonly partial struct LootableContainer
		{
			public const uint InteractingPlayer = 0xC0; // EFT.IPlayer
			public const uint ItemOwner = 0x130; // -.\uEFDF
			public const uint Template = 0x138; // String
		}

		public readonly partial struct LootableContainerItemOwner
		{
			public const uint RootItem = 0xD0; // EFT.InventoryLogic.Item
		}

		public readonly partial struct LootItem
		{
			public const uint Template = 0x58; // EFT.InventoryLogic.ItemTemplate
			public const uint StackObjectsCount = 0x7C; // Int32
			public const uint Version = 0x80; // Int32
		}

		public readonly partial struct LootItemMod
		{
			public const uint Grids = 0x90; // -.\uF002[]
			public const uint Slots = 0x98; // EFT.InventoryLogic.Slot[]
		}

		public readonly partial struct LootItemModGrids
		{
			public const uint ItemCollection = 0x48; // -.\uF004
		}

		public readonly partial struct LootItemModGridsItemCollection
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Item>
		}

		public readonly partial struct LootItemWeapon
		{
			public const uint FireMode = 0xB8; // EFT.InventoryLogic.FireModeComponent
			public const uint Chambers = 0xC8; // EFT.InventoryLogic.Slot[]
			public const uint _magSlotCache = 0xE0; // EFT.InventoryLogic.Slot
		}

		public readonly partial struct FireModeComponent
		{
			public const uint FireMode = 0x28; // System.Byte
		}

		public readonly partial struct LootItemMagazine
		{
			public const uint Cartridges = 0xB8; // EFT.InventoryLogic.StackSlot
		}

		public readonly partial struct MagazineClass
		{
			public const uint StackObjectsCount = 0x7C; // Int32
		}

		public readonly partial struct StackSlot
		{
			public const uint _items = 0x28; // System.Collections.Generic.List<Item>
			public const uint MaxCount = 0x50; // Int32
		}

		public readonly partial struct ItemTemplate
		{
			public const uint ShortName = 0x18; // String
			public const uint _id = 0x60; // EFT.MongoID
		}

		public readonly partial struct ModTemplate
		{
			public const uint Velocity = 0x180; // Single
		}

		public readonly partial struct AmmoTemplate
		{
			public const uint InitialSpeed = 0x1E0; // Single
			public const uint BallisticCoeficient = 0x1F4; // Single
			public const uint BulletMassGram = 0x27C; // Single
			public const uint BulletDiameterMilimeters = 0x280; // Single
		}

		public readonly partial struct WeaponTemplate
		{
			public const uint Velocity = 0x26C; // Single
		}

		public readonly partial struct PlayerBody
		{
			public const uint SkeletonRootJoint = 0x30; // Diz.Skinning.Skeleton
			public const uint BodySkins = 0x48; // System.Collections.Generic.Dictionary<Int32, LoddedSkin>
			public const uint _bodyRenderers = 0x58; // -.\uE450[]
			public const uint SlotViews = 0x70; // -.\uE3E0<Int32, \uE001>
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

		public readonly partial struct OpticCameraManagerContainer
		{
			public const uint Instance = 0x0; // -.\uF208
			public const uint OpticCameraManager = 0x10; // -.\uF20C
			public const uint FPSCamera = 0x68; // UnityEngine.Camera
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
			public const uint _template = 0x20; // -.\uEFFA
			public const uint ScopesSelectedModes = 0x30; // System.Int32[]
			public const uint SelectedScope = 0x38; // Int32
		}

		public readonly partial struct SightInterface
		{
			public const uint Zooms = 0x1A8; // System.Single[]
		}

		public readonly partial struct NetworkGame
		{
			public const uint NetworkGameData = 0x70; // -.\uE9E5
		}

		public readonly partial struct NetworkGameData
		{
			public const uint raidMode = 0x4C; // System.Int32
		}
	}

	public readonly partial struct Enums
	{
		public enum ERaidMode
		{
			Online = 0,
			Local = 1,
			Coop = 2,
			OverRun = 3,
			TeamFight = 4,
			LastHero = 5,
			FinalRun = 6,
			OneManArmy = 7,
			Duel = 8,
			ShootOut = 9,
			ShootOutSolo = 10,
			ShootOutDuo = 11,
			ShootOutTrio = 12,
			BlastGang = 13,
			CheckPoint = 14,
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

		public enum ArmbandColorType
		{
			red = 1,
			fuchsia = 2,
			yellow = 3,
			green = 4,
			azure = 5,
			white = 6,
			blue = 7,
			grey = 8,
		}

		public enum ECameraType
		{
			Default = 0,
			Spectator = 1,
			UIBackground = 2,
			KillCamera = 3,
		}
	}
}
