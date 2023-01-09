using System;

using MelonLoader;
using HarmonyLib;

using Vertigo2.Weapons;
using Vertigo2.Interaction;

[assembly: MelonInfo(typeof(VertigoRemastered_ProTube.VertigoRemastered_ProTube), "VertigoRemastered_ProTube", "1.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("Zulubo Productions", "vertigo")]


namespace VertigoRemastered_ProTube
{
    public class VertigoRemastered_ProTube : MelonMod
    {
        // private static int leftHand = ((int)SteamVR_Input_Sources.LeftHand);
        public override void OnInitializeMelon()
        {
            InitializeProTube();
        }

        private static void InitializeProTube()
        {
            MelonLogger.Msg("Initializing ProTube gear...");
            ForceTubeVRInterface.InitAsync(true);
        }


        #region Weapons

        [HarmonyPatch(typeof(Gun), "ShootHaptics")]
        public class bhaptics_GunFeedback
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance, float length, float power)
            {
                float intensity = Math.Max(power * 2.0f, 1.0f);
                byte kickPower = (byte)(int)(intensity * 255);
                ForceTubeVRInterface.Kick(kickPower, ForceTubeVRChannel.all);
            }
        }

        [HarmonyPatch(typeof(PlasmaSword), "OnImpact")]
        public class bhaptics_PlasmaSwordOnOmpact
        {
            [HarmonyPostfix]
            public static void Postfix(PlasmaSword __instance, float normalizedSpeed)
            {
                float intensity = Math.Max((1.0f - (1.0f - normalizedSpeed) * 0.5f), 1.0f);
                byte kickPower = (byte)(int)(intensity * 255);
                ForceTubeVRInterface.Rumble(kickPower, 200f, ForceTubeVRChannel.all);
            }
        }

        [HarmonyPatch(typeof(PlasmaSword), "OnDeflect")]
        public class bhaptics_PlasmaSwordOnDeflect
        {
            [HarmonyPostfix]
            public static void Postfix(PlasmaSword __instance)
            {
                ForceTubeVRInterface.Rumble(200, 100f, ForceTubeVRChannel.all);
            }
        }

        [HarmonyPatch(typeof(RocketLauncher), "ShootHaptics")]
        public class bhaptics_ShootRocketHaptics
        {
            [HarmonyPostfix]
            public static void Postfix(VertigoInteractable handle)
            {
                ForceTubeVRInterface.Shoot(200, 255, 300f, ForceTubeVRChannel.all);
            }
        }


        #endregion

    }
}
