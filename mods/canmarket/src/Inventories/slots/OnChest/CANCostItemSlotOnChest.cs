﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace canmarket.src.Inventories
{
    public class CANCostItemSlotOnChest: CANCostItemSlotAbstract
    {
        public CANCostItemSlotOnChest(InventoryBase inventory) : base(inventory)
        {
        }
        protected override void ActivateSlotLeftClick(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
        {
            
            if(!op.ActingPlayer.PlayerName.Equals((inventory as InventoryCANMarketOnChest).be.ownerName))
            {
                return;
            }
            if (sourceSlot.Itemstack == null)
            {
                itemstack = null;
                return;
            }
            //.Collectible.Equals(inventory[curSlot].Itemstack, inventory[0].Itemstack, GlobalConstants.IgnoredStackAttributes)
            if (itemstack != null)
            {
                //Slot already has the same item, just try to add stacksize from source or set maximum
                if (itemstack.Collectible.Equals(itemstack, sourceSlot.Itemstack, Config.Current.IGNORED_STACK_ATTRIBTES_ARRAY.Val))
                {
                   // itemstack.StackSize += sourceSlot.StackSize;
                    itemstack.StackSize = Math.Min(itemstack.StackSize + sourceSlot.StackSize, itemstack.Collectible.MaxStackSize);
                    this.MarkDirty();
                    return;
                }
                else
                {
                    //Just ignore because this item is different
                    return;
                }
            }
            //Slot is empty, just fill it with source slot item
            else
            {
                itemstack = sourceSlot.Itemstack.Clone();
                //just to be calm
                itemstack.StackSize = Math.Min(itemstack.StackSize, itemstack.Collectible.MaxStackSize);
                this.MarkDirty();
                return;
            }          
        }
        protected override void ActivateSlotRightClick(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
        {
            if (!op.ActingPlayer.PlayerName.Equals((inventory as InventoryCANMarketOnChest).be.ownerName))
            {
                return;
            }
            //slot is already empty
            if (itemstack == null)
            {
                return;
            }
            else
            {
                if (itemstack.StackSize > 1)
                {
                    itemstack.StackSize /= 2;
                    this.MarkDirty();
                }
                else
                {
                    itemstack = null;
                    this.MarkDirty();
                }
            }
        }
    }
}
