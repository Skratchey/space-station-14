﻿using System;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Components.UserInterface;
using Robust.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.PDA
{
    public class SharedPDAComponent : Component
    {
        public override string Name => "PDA";
        public override uint? NetID => ContentNetIDs.PDA;

    }

    [Serializable, NetSerializable]
    public sealed class PDAToggleFlashlightMessage : BoundUserInterfaceMessage
    {
        public PDAToggleFlashlightMessage()
        {

        }
    }

    [Serializable, NetSerializable]
    public sealed class PDAEjectIDMessage : BoundUserInterfaceMessage
    {
        public PDAEjectIDMessage()
        {

        }
    }

    [Serializable, NetSerializable]
    public class PDAUBoundUserInterfaceState : BoundUserInterfaceState
    {

    }

    [Serializable, NetSerializable]
    public sealed class PDAUpdateState : PDAUBoundUserInterfaceState
    {
        public bool FlashlightEnabled;
        public PDAIdInfoText PDAOwnerInfo;
        public UplinkAccountData Account;
        public UplinkListingData[] Listings;

        public PDAUpdateState(bool isFlashlightOn, PDAIdInfoText ownerInfo)
        {
            FlashlightEnabled = isFlashlightOn;
            PDAOwnerInfo = ownerInfo;
        }

        public PDAUpdateState(bool isFlashlightOn, PDAIdInfoText ownerInfo, UplinkAccountData accountData)
        {
            FlashlightEnabled = isFlashlightOn;
            PDAOwnerInfo = ownerInfo;
            Account = accountData;
        }

        public PDAUpdateState(bool isFlashlightOn, PDAIdInfoText ownerInfo, UplinkAccountData accountData, UplinkListingData[] listings)
        {
            FlashlightEnabled = isFlashlightOn;
            PDAOwnerInfo = ownerInfo;
            Account = accountData;
            Listings = listings;
        }
    }

    [Serializable, NetSerializable]
    public sealed class PDAUplinkBuyListingMessage : BoundUserInterfaceMessage
    {
        public string ItemId;

        public PDAUplinkBuyListingMessage(string itemId)
        {
            ItemId = itemId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class PDAUplinkBuySuccessMessage : ComponentMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class PDAUplinkInsufficientFundsMessage : ComponentMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class PDARequestUpdateInterfaceMessage : BoundUserInterfaceMessage
    {
        public PDARequestUpdateInterfaceMessage()
        {

        }
    }

    [Serializable, NetSerializable]
    public struct PDAIdInfoText
    {
        public string ActualOwnerName;
        public string IdOwner;
        public string JobTitle;
    }

    [Serializable, NetSerializable]
    public enum PDAVisuals
    {
        FlashlightLit,
        IDCardInserted
    }

    [Serializable, NetSerializable]
    public enum PDAUiKey
    {
        Key
    }

    public class UplinkAccount
    {
        public event Action<UplinkAccount> BalanceChanged;
        public EntityUid AccountHolder;
        public int Balance { get; private set; }

        public UplinkAccount(EntityUid uid, int startingBalance)
        {
            AccountHolder = uid;
            Balance = startingBalance;
        }

        public bool ModifyAccountBalance(int newBalance)
        {
            if (newBalance < 0)
            {
                return false;
            }
            Balance = newBalance;
            BalanceChanged?.Invoke(this);
            return true;

        }
    }

    [Serializable, NetSerializable]
    public class UplinkAccountData
    {
        public EntityUid DataAccountHolder;
        public int DataBalance;

        public UplinkAccountData(EntityUid dataAccountHolder, int dataBalance)
        {
            DataAccountHolder = dataAccountHolder;
            DataBalance = dataBalance;
        }
    }

    [Serializable, NetSerializable]
    public class UplinkListingData : ComponentState, IEquatable<UplinkListingData>
    {
        public string ItemId;
        public int Price;
        public UplinkCategory Category;
        public string Description;
        public string ListingName;

        public UplinkListingData(string listingName,string itemId,
            int price, UplinkCategory category,
            string description) : base(ContentNetIDs.PDA)
        {
            ListingName = listingName;
            Price = price;
            Category = category;
            Description = description;
            ItemId = itemId;
        }

        public bool Equals(UplinkListingData other)
        {
            if (other == null)
            {
                return false;
            }

            return ItemId == other.ItemId;
        }
    }
}
