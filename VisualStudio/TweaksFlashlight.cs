﻿using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks;

internal class TweaksFlashlight
{
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Awake))]
    private static class FlashlightCustomization
    {
        private static void Prefix(FlashlightItem __instance)
        {
            if (Settings.Instance.RandomizeFlashlightCharge)
            {
                __instance.m_CurrentBatteryCharge = UnityEngine.Random.Range(0f, 1f);
                Logging.Log($"Randomized battery charge: {__instance.m_CurrentBatteryCharge}");
            }
        }
    }


    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.GetNormalizedCharge))]
    private static class FlashlightKeepBatteryCharge
    {
        private static bool Prefix(FlashlightItem __instance, ref float __result)
        {
            if (!Settings.Instance.EnableFlashlightWithoutAurora)
            {
                return true;
            }

            __result = __instance.m_CurrentBatteryCharge;
            return false;
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.IsLit))]
    private static class FlashlightFunctionality
    {
        private static bool Prefix(FlashlightItem __instance, ref bool __result)
        {
            if (Settings.Instance.HighBeamOnlyDuringAurora && __instance.m_State == FlashlightItem.State.High && !GameManager.GetAuroraManager().AuroraIsActive())
            {
                GameAudioManager.PlayGUIError();
                HUDMessage.AddMessage(Localization.Get("GAMEPLAY_StateHighFail"), false, false);
                __result = __instance.IsOn();
                return true;
            }

            __result = __instance.IsOn();
            return false;
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Update))]
    private static class FlashlightBatteryDrain
    {
        private static void Postfix(FlashlightItem __instance)
        {
            float tODHours = GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime);

            if (!GameManager.GetAuroraManager().AuroraIsActive())
            {
                if (__instance.m_State == FlashlightItem.State.Low)
                {
                    __instance.m_CurrentBatteryCharge -= tODHours / __instance.m_LowBeamDuration;
                }
                else if (!Settings.Instance.HighBeamOnlyDuringAurora && __instance.m_State == FlashlightItem.State.High)
                {
                    __instance.m_CurrentBatteryCharge -= tODHours / __instance.m_HighBeamDuration;
                }
                if (__instance.m_CurrentBatteryCharge <= 0f)
                {
                    __instance.m_CurrentBatteryCharge = 0f;
                    __instance.m_State = FlashlightItem.State.Off;
                }
            }

            if (Settings.Instance.InfiniteBattery)
            {
                __instance.m_CurrentBatteryCharge = 1f;
            }

            if (Settings.Instance.ExperimentalFeatures)
            {
                if (__instance.m_GearItem != null && __instance.m_GearItem.name == "GEAR_Flashlight_LongLasting")
                {
                    __instance.m_LowBeamDuration = Settings.Instance.MinersFlashlightLowBeamDuration;
                    __instance.m_HighBeamDuration = Settings.Instance.MinersFlashlightHighBeamDuration;
                    __instance.m_RechargeTime = Settings.Instance.MinersFlashlightRechargeTime;
                }
                else
                {
                    __instance.m_LowBeamDuration = Settings.Instance.FlashlightLowBeamDuration;
                    __instance.m_HighBeamDuration = Settings.Instance.FlashlightHighBeamDuration;
                    __instance.m_RechargeTime = Settings.Instance.FlashlightRechargeTime;
                }
            }
            else
            {
                if (__instance.m_GearItem != null && __instance.m_GearItem.name == "GEAR_Flashlight_LongLasting")
                {
                    __instance.m_LowBeamDuration = 1.5f;
                    __instance.m_HighBeamDuration = 0.08333334f;
                    __instance.m_RechargeTime = 1.75f;
                }
                else
                {
                    __instance.m_LowBeamDuration = 1f;
                    __instance.m_HighBeamDuration = 0.08333334f;
                    __instance.m_RechargeTime = 2f;
                }
            }
        }
    }

    [HarmonyPatch(typeof(LightRandomIntensity), nameof(LightRandomIntensity.Update))]
    private static class FlashlightFlicker
    {
        private static bool Prefix(LightRandomIntensity __instance)
        {
            if ((__instance.gameObject.name == "LightIndoors" || __instance.gameObject.name == "LightOutdoors" || __instance.gameObject.name == "LightExtend") && !GameManager.GetAuroraManager().AuroraIsActive() && Settings.Instance.EnableFlashlightWithoutAurora)
            {
                return false;
            }

            return true;
        }

        private static void Postfix(LightRandomIntensity __instance)
        {
            if (__instance.gameObject.name == "LightIndoors" || __instance.gameObject.name == "LightOutdoors" || __instance.gameObject.name == "LightExtend")
            {
                Light lightComponent = __instance.gameObject.GetComponent<Light>();

                if (lightComponent != null)
                {
                    switch (Settings.Instance.DefaultFlashlightColor)
                    {
                        case FlashlightColor.Custom:
                            lightComponent.color = new Color(Settings.Instance.FlashlightLightRed / 255,
                                                             Settings.Instance.FlashlightLightGreen / 255,
                                                             Settings.Instance.FlashlightLightBlue / 255);
                            break;
                        case FlashlightColor.Default:
                            lightComponent.color = new Color(0.7215686f, 0.8117647f, 0.9176471f);
                            break;
                        case FlashlightColor.Red:
                            lightComponent.color = Color.red;
                            break;
                        case FlashlightColor.Green:
                            lightComponent.color = Color.green;
                            break;
                        case FlashlightColor.Blue:
                            lightComponent.color = Color.blue;
                            break;
                        case FlashlightColor.Yellow:
                            lightComponent.color = Color.yellow;
                            break;
                        case FlashlightColor.Purple:
                            lightComponent.color = new Color(0.5f, 0f, 0.5f);
                            break;
                        case FlashlightColor.Orange:
                            lightComponent.color = new Color(1f, 0.6470588f, 0f);
                            break;
                        case FlashlightColor.White:
                            lightComponent.color = Color.white;
                            break;
                    }
                }
            }
        }
    }

    // Below is currently experimental, trying to figure out how to retain the lights when dropped if the state is still low.

    //[HarmonyPatch(typeof(GearItem), nameof(GearItem.Drop))]
    //private static class Testintg1
    //{
    //    private static void Postfix(GearItem __instance)
    //    {
    //        Logging.Log("GearItem.Drop Postfix called.");

    //        if (__instance.m_FlashlightItem == null)
    //        {
    //            Logging.LogWarning("Dropped item is not a flashlight.");
    //            return;
    //        }

    //        if (__instance.m_FlashlightItem.IsOn())
    //        {
    //            Logging.Log("Flashlight is on. Attempting to maintain state upon drop.");

    //            // Attempt to maintain the flashlight's current state when dropped
    //            try
    //            {
    //                __instance.m_FlashlightItem.TurnOn(); // Assuming this turns it to a default on state
    //                                                      // OR
    //                                                      // __instance.m_FlashlightItem.SetState(__instance.m_FlashlightItem.m_State); // if there's a method like SetState

    //                Logging.Log("Flashlight state maintained after dropping.");
    //            }
    //            catch (Exception ex)
    //            {
    //                Logging.LogError($"Error while maintaining flashlight state: {ex.Message}");
    //            }
    //        }
    //        else
    //        {
    //            Logging.Log("Dropped flashlight item is not currently on. No action taken.");
    //        }
    //    }
    //}
}