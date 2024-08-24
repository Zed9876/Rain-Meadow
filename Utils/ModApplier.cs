﻿using Menu;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RainMeadow.RainMeadow;
using static System.Net.Mime.MediaTypeNames;

namespace RainMeadow
{
    internal class ModApplier : ModManager.ModApplyer
    {
        public DialogAsyncWait dialogBox;
        public DialogNotify checkUserConfirmation; 
        public DialogNotify requiresRestartDialog;
        public DialogNotify restartText;
        public string modMismatchString;

        private readonly Menu.Menu menu;
        private List<ModManager.Mod> modsToEnable;
        private List<ModManager.Mod> modsToDisable;


        public event Action<ModApplier> OnFinish;

        public ModApplier(ProcessManager manager, List<bool> pendingEnabled, List<int> pendingLoadOrder) : base(manager, pendingEnabled, pendingLoadOrder)
        {
            On.RainWorld.Update += RainWorld_Update;
            On.ModManager.ModApplyer.ApplyModsThread += ModApplyer_ApplyModsThread;
            menu = (Menu.Menu)manager.currentMainLoop;
            this.modsToDisable = new List<ModManager.Mod>();
            this.modsToEnable = new List<ModManager.Mod>();

        }

        private void ModApplyer_ApplyModsThread(On.ModManager.ModApplyer.orig_ApplyModsThread orig, ModManager.ModApplyer self)
        {
            if (modsToDisable.Count > 0)
            {

                for (int i = 0; i < modsToDisable.Count; i++)
                {

                    int installedModIndex = ModManager.InstalledMods.FindIndex(mod => mod.id == modsToDisable[i].id);
                    if (installedModIndex != -1)
                    {
                        ModManager.InstalledMods[installedModIndex].enabled = false;
                        self.pendingEnabled[installedModIndex] = false;
                    }
                }
            }

            if (modsToEnable.Count > 0)
            {
                for (int i = 0; i < modsToEnable.Count; i++)
                {
                    int installedModIndex = ModManager.InstalledMods.FindIndex(mod => mod.id == modsToEnable[i].id);
                    if (installedModIndex != -1)
                    {
                        // ModManager.InstalledMods[installedModIndex].enabled = false;
                        self.pendingEnabled[installedModIndex] = true;
                    }
                }
            }
            orig(self);

        }

        private void RainWorld_Update(On.RainWorld.orig_Update orig, RainWorld self)
        {
            orig(self);

            Update();
        }

        public new void Update()
        {

            base.Update();


            dialogBox?.SetText(menu.Translate("mod_menu_apply_mods") + Environment.NewLine + statusText);


            if (IsFinished())
            {
                On.RainWorld.Update -= RainWorld_Update;
                if (dialogBox != null)
                {
                    dialogBox.RemoveSprites();
                    manager.dialog = null;
                    manager.ShowNextDialog();
                    dialogBox = null;
                }

                OnFinish?.Invoke(this);
                manager.rainWorld.options.Save();
            }
        }

        public void ShowConfirmation(List<ModManager.Mod> modsToEnable, List<ModManager.Mod> modsToDisable, List<string> unknownMods)
        {

            modMismatchString = menu.Translate("Mod mismatch detected.") + Environment.NewLine;


            if (modsToEnable.Count > 0)
            {
                modMismatchString += Environment.NewLine + menu.Translate("Mods currently enabled: ") + string.Join(", ", modsToEnable.ConvertAll(mod => mod.LocalizedName));
                this.modsToEnable = modsToEnable;
            }
            if (modsToDisable.Count > 0)
            {
                modMismatchString += Environment.NewLine + menu.Translate("Mods that need to be disabled: ") + string.Join(", ", modsToDisable.ConvertAll(mod => mod.LocalizedName));
                this.modsToDisable = modsToDisable;
            }
            if (unknownMods.Count > 0)
            {
                modMismatchString += Environment.NewLine + menu.Translate("Unable to find the following mods, please install them: ") + string.Join(", ", unknownMods);
            }
            // modMismatchString += Environment.NewLine + Environment.NewLine + menu.Translate("Rain World may be restarted for these changes to take effect");

            modMismatchString += Environment.NewLine + Environment.NewLine + menu.Translate("You must match the host's mod list to proceed");

            Action confirmProceed = () =>
            {
                if (OnlineManager.instance == null)
                {
                    manager.dialog = null;
                    manager.ShowNextDialog();
                    requiresRestartDialog = null;
                    OnlineManager.LeaveLobby();
                    manager.RequestMainProcessSwitch(RainMeadow.Ext_ProcessID.LobbySelectMenu);
                }
                else
                {
                   
                    Start(filesInBadState);
                }
            };

            Action cancelProceed = () =>
            {
                manager.dialog = null;
                requiresRestartDialog = null;
                OnlineManager.LeaveLobby();
                manager.RequestMainProcessSwitch(RainMeadow.Ext_ProcessID.LobbySelectMenu);

            };


            if (OnlineManager.instance == null)
            {
                modMismatchString = "Error joining lobby";
            }


            checkUserConfirmation = new DialogNotify(modMismatchString, new Vector2(480f, 320f), manager,  cancelProceed);

            manager.ShowDialog(checkUserConfirmation);
        }

        public new void Start(bool filesInBadState)
        {

            if (requiresRestartDialog != null)
            {
                manager.dialog = null;
                manager.ShowNextDialog();
                requiresRestartDialog = null;
                OnlineManager.LeaveLobby();
                manager.RequestMainProcessSwitch(RainMeadow.Ext_ProcessID.LobbySelectMenu);
            }
            base.Start(filesInBadState);

        }
    }
}
