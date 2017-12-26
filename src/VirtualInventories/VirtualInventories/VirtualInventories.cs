using System;
using fNbt;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace VirtualInventories
{
    [Plugin(PluginName = "VirtualInventories", PluginVersion = "1.0.0", Author = "iksaku", Description = "MiNET implementation of inventories that doesn't exist physically in MC World")]
    public class VirtualInventories : Plugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VirtualInventories));
        
        protected override void OnEnable()
        {
            Log.Info("VirtualInventories Plugin Loaded!");
        }

        [Command(Name = "vi", Description = "Open a Chest")]
        public void OpenInventory(Player player)
        {
            //Log.Info("Command Executed!");
            player.SendMessage("Opening Chest...");
            
            /*BlockCoordinates coords = (BlockCoordinates) player.KnownPosition;
            coords.Y = 0;*/
            BlockCoordinates coords = new BlockCoordinates(0);

            //Block past = player.Level.GetBlock(coords);

            McpeUpdateBlock chest = Package<McpeUpdateBlock>.CreateObject();
            chest.blockId = 54;
            chest.coordinates = coords;
            chest.blockMetaAndPriority = 0 & 15;
            player.SendPackage(chest);

            ChestBlockEntity blockEntity = new ChestBlockEntity {Coordinates = coords};
            NbtCompound compound = blockEntity.GetCompound();
            compound["CustomName"] = new NbtString("CustomName", "§5§k--§r §l§o§2Virtual Chest§r §5§k--§r");
            //player.Level.SetBlockEntity(blockEntity);
            McpeBlockEntityData chestEntity = Package<McpeBlockEntityData>.CreateObject();
            chestEntity.namedtag = new Nbt
            {
                NbtFile = new NbtFile
                {
                    BigEndian = false,
                    UseVarInt = true,
                    RootTag = compound
                }
            };
            chestEntity.coordinates = coords;
            player.SendPackage(chestEntity);

            //player.OpenInventory(coords);
            Inventory inventory = new Inventory(0, blockEntity, 1, new NbtList())
            {
                Type = 0,
                WindowsId = 10
            };
            
            //inventory.InventoryChange += new Action<Player, MiNET.Inventory, byte, Item>(player.OnInventoryChange);
            inventory.AddObserver(player);
            McpeContainerOpen mcpeContainerOpen = Package<McpeContainerOpen>.CreateObject(1L);
            mcpeContainerOpen.windowId = inventory.WindowsId;
            mcpeContainerOpen.type = inventory.Type;
            mcpeContainerOpen.coordinates = coords;
            mcpeContainerOpen.unknownRuntimeEntityId = 1L;
            player.SendPackage((Package) mcpeContainerOpen);
            McpeInventoryContent inventoryContent = Package<McpeInventoryContent>.CreateObject(1L);
            inventoryContent.inventoryId = (uint) inventory.WindowsId;
            inventoryContent.input = inventory.Slots;
            player.SendPackage((Package) inventoryContent);
        }
    }
}