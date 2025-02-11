﻿using System;
using System.Collections.Generic;

namespace RainMeadow
{
    internal class ArenaLobbyData : OnlineResource.ResourceData
    {
        public ArenaLobbyData() { }

        public override ResourceDataState MakeState(OnlineResource resource)
        {
            return new State(this, resource);
        }

        internal class State : ResourceDataState
        {
            [OnlineField]
            public bool isInGame;
            [OnlineField]
            public bool allPlayersReadyLockLobby;
            [OnlineField]
            public List<string> playList;
            [OnlineField]
            public List<ushort> arenaSittingOnlineOrder;
            [OnlineField]
            public bool returnToLobby;
            [OnlineField]
            public Dictionary<string, int> onlineArenaSettingsInterfaceMultiChoice;
            [OnlineField]
            public Dictionary<string, bool> onlineArenaSettingsInterfaceBool;
            [OnlineField]
            public Dictionary<string, int> playersChoosingSlugs;
            [OnlineField]
            public Dictionary<string, int> playerResultColors;
            [OnlineField]
            public bool countdownInitiatedHoldFire;
            [OnlineField]
            public int playerEnteredGame;
            [OnlineField]
            public int clientsAreReadiedUp;
            [OnlineField]
            public int arenaSetupTime;
            [OnlineField]
            public bool sainot;
            public State() { }
            public State(ArenaLobbyData arenaLobbyData, OnlineResource onlineResource)
            {
                ArenaCompetitiveGameMode arena = (onlineResource as Lobby).gameMode as ArenaCompetitiveGameMode;
                isInGame = RWCustom.Custom.rainWorld.processManager.currentMainLoop is RainWorldGame;
                playList = arena.playList;
                arenaSittingOnlineOrder = arena.arenaSittingOnlineOrder;
                allPlayersReadyLockLobby = arena.allPlayersReadyLockLobby;
                returnToLobby = arena.returnToLobby;
                onlineArenaSettingsInterfaceMultiChoice = arena.onlineArenaSettingsInterfaceMultiChoice;
                onlineArenaSettingsInterfaceBool = arena.onlineArenaSettingsInterfaceeBool;
                playersChoosingSlugs = arena.playersInLobbyChoosingSlugs;
                countdownInitiatedHoldFire = arena.countdownInitiatedHoldFire;
                playerResultColors = arena.playerResultColors;
                playerEnteredGame = arena.playerEnteredGame;
                clientsAreReadiedUp = arena.clientsAreReadiedUp;
                arenaSetupTime = arena.setupTime;
                sainot = arena.sainot;

            }

            public override void ReadTo(OnlineResource.ResourceData data, OnlineResource resource)
            {
                var lobby = (resource as Lobby);
                (lobby.gameMode as ArenaCompetitiveGameMode).isInGame = isInGame;
                (lobby.gameMode as ArenaCompetitiveGameMode).playList = playList;
                (lobby.gameMode as ArenaCompetitiveGameMode).arenaSittingOnlineOrder = arenaSittingOnlineOrder;
                (lobby.gameMode as ArenaCompetitiveGameMode).allPlayersReadyLockLobby = allPlayersReadyLockLobby;
                (lobby.gameMode as ArenaCompetitiveGameMode).returnToLobby = returnToLobby;
                (lobby.gameMode as ArenaCompetitiveGameMode).onlineArenaSettingsInterfaceMultiChoice = onlineArenaSettingsInterfaceMultiChoice;
                (lobby.gameMode as ArenaCompetitiveGameMode).onlineArenaSettingsInterfaceeBool = onlineArenaSettingsInterfaceBool;
                (lobby.gameMode as ArenaCompetitiveGameMode).playersInLobbyChoosingSlugs = playersChoosingSlugs;
                (lobby.gameMode as ArenaCompetitiveGameMode).countdownInitiatedHoldFire = countdownInitiatedHoldFire;
                (lobby.gameMode as ArenaCompetitiveGameMode).playerResultColors = playerResultColors;
                (lobby.gameMode as ArenaCompetitiveGameMode).playerEnteredGame = playerEnteredGame;
                (lobby.gameMode as ArenaCompetitiveGameMode).clientsAreReadiedUp = clientsAreReadiedUp;
                (lobby.gameMode as ArenaCompetitiveGameMode).setupTime = arenaSetupTime;
                (lobby.gameMode as ArenaCompetitiveGameMode).sainot = sainot;


            }

            public override Type GetDataType() => typeof(ArenaLobbyData);
        }
    }
}
