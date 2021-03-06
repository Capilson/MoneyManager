﻿#region

using System;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

#endregion

namespace MoneyManager.Business.Logic.Tile {
    public class SpendingTile : Tile, ISecondTile {
        public const string Id = "AddSpendingTile";

        public bool Exists {
            get { return Exists(Id); }
        }

        public async void Create() {
            await Create(new SecondaryTile(
                Id,
                Translation.GetTranslation("AddSpendingTileText"),
                "intake",
                new Uri("ms-appx:///Images/spendingTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async void Remove() {
            await Remove(new SecondaryTile(Id));
        }
    }
}