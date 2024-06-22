using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Diagnostics;

namespace ShowCharDiff
{
    [ApiVersion(2, 1)]
    public class ShowCharDiff : TerrariaPlugin
    {

        public override string Author => "Onusai";
        public override string Description => "Shows character difficulty when player joins";
        public override string Name => "ShowCharDiff";
        public override Version Version => new Version(1, 0, 0, 0);

        public string[] diff = { "softcore", "mediumcore", "hardcore" };

        public ShowCharDiff(Main game) : base(game) { }

        public override void Initialize()
        {

            ServerApi.Hooks.GameInitialize.Register(this, OnGameLoad);
        }
        
        void OnGameLoad(EventArgs e)
        {
            ServerApi.Hooks.NetGreetPlayer.Register(this, PlayerJoin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnGameLoad);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, PlayerJoin);
            }
            base.Dispose(disposing);
        }


        void PlayerJoin(GreetPlayerEventArgs e)
        {
            var player = TShock.Players[e.Who];
            if (player == null) return;

            player.SilentJoinInProgress = true;
            TShock.Utils.Broadcast(String.Format("[{1}] {0} has joined", player.Name, diff[player.Difficulty]), Color.Yellow);
        }
    }
}