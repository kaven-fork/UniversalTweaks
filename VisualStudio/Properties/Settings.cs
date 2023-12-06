﻿using UniversalTweaks.Utilities;

namespace UniversalTweaks.Properties;

internal class Settings : JsonModSettings
{
    internal static Settings Instance { get; } = new();

    #region General Tweaks
    [Section("General Tweaks")]

    [Name("Breath Visibility")]
    [Description("Toggle the visual breath effect on or off.")]
    public bool BreathVisibility = true;

    [Name("Promotional Main Menu Items")]
    [Description("Removes the promotional WINTERMUTE and Expansion menu items.")]
    public bool RemoveMainMenuItems = false;

    [Name("Permanent Crosshair")]
    [Description("Keeps the crosshair visible at all times.")]
    public bool PermanentCrosshair = false;

    [Name("Remove Headache from Pies")]
    [Description("Removes the headache debuff from Peach and Rosehip Pies.")]
    public bool RemoveHeadacheDebuffFromPies = false;

    [Name("Revolver Handling Improvements")]
    [Description("Allows movement while aiming with the revolver.")]
    public bool RevolverImprovements = false;

    [Name("Snow Shelter Decay Rate")]
    [Description("Adjust the daily decay rate of snow shelters. Range: 50 (twice as slow) to 100 (normal).")]
    [Slider(50, 100)]
    public int SnowShelterDailyDecayRate = 100;

    [Name("Toilet Water Potability")]
    [Description("Sets whether water obtained from toilets is potable or non-potable.")]
    [Choice("Potable", "Non-Potable")]
    public int ToiletWaterQuality = 0;
    #endregion

    #region Flashlight Tweaks
    [Section("Flashlight Tweaks")]

    [Name("Aurora Flickering")]
    [Description("Alters the flashlight's flickering behavior by only flickering during an aurora if enabled.")]
    public bool AuroraFlickering = false;

    [Name("Battery Randomization")]
    [Description("Sets the flashlight to start with a random battery charge level.")]
    public bool BatteryRandomization = false;

    [Name("Extended Functionality")]
    [Description("Allows the flashlight to function anytime, not just during auroras.")]
    public bool ExtendedFunctionality = false;

    [Name("High-Beam Restrictions")]
    [Description("Restricts high beam functionality to aurora events only.")]
    public bool HighBeamRestrictions = false;

    [Name("Light Color")]
    [Description("Changes the color of the flashlight's beam.")]
    public FlashlightColor LightColor = FlashlightColor.Default;

    [Name("Red Value")]
    [Slider(0, 255)]
    public int RedValue = 0;

    [Name("Green Value")]
    [Slider(0, 255)]
    public int GreenValue = 0;

    [Name("Blue Value")]
    [Slider(0, 255)]
    public int BlueValue = 0;

    [Name("Flashlight Low Beam Duration")]
    [Description("Adjusts the flashlight's low beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float FlashlightLowBeamDuration = 1f;

    [Name("Flashlight High Beam Duration")]
    [Description("Adjusts the flashlight's high beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float FlashlightHighBeamDuration = 0.08333334f;

    [Name("Flashlight Recharge Time")]
    [Description("Sets the recharge time for the flashlight's battery. Range: 0 (fast) to 2 (slow).")]
    [Slider(0f, 2f, 20)]
    public float FlashlightRechargeTime = 2f;

    [Name("Infinite Battery")]
    [Description("Ensures the flashlight's battery never depletes, allowing unlimited use without recharging.")]
    public bool InfiniteBattery = false;

    [Name("Miner's Flashlight Low Beam Duration")]
    [Description("Adjusts the miner's flashlight's low beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float MinersFlashlightLowBeamDuration = 1.5f;

    [Name("Miner's Flashlight High Beam Duration")]
    [Description("Adjusts the miner's flashlight's high beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float MinersFlashlightHighBeamDuration = 0.08333334f;

    [Name("Miner's Flashlight Recharge Time")]
    [Description("Sets the recharge time for the miner's flashlight's battery. Range: 0 (fast) to 2 (slow).")]
    [Slider(0f, 2f, 20)]
    public float MinersFlashlightRechargeTime = 1.75f;
    #endregion

    #region Spray Paint Tweaks
    [Section("Spray Paint Tweaks")]

    [Name("Decal Overlap Leniency")]
    [Description("Set the leniency for placing decals. Range: 0 (strict) to 1 (lenient).")]
    [Slider(0f, 1f, 11)]
    public float DecalOverlapLeniency = 0.2f;

    [Name("Glowing Decals")]
    [Description("Toggle the glow effect for spray paint decals.")]
    public bool GlowingDecals = false;

    [Name("Glow Brightness Multiplier")]
    [Description("Adjust the brightness of the glow effect. Range: 0.5 (low) to 1.5 (high).")]
    [Slider(0.5f, 1.5f, 11)]
    public float GlowingDecalMultiplier = 1f;
    #endregion

    #region Experimental Tweaks
    [Section("Experimental Tweaks")]

    [Name("Experimental / Cheat Tweaks")]
    [Description("USE AT YOUR OWN RISK! Enables cheats and tweaks that aren't quite polished.")]
    public bool ExperimentalAndCheatTweaks = false;

    [Name("First-Person Handedness")]
    [Description("Alters the handedness of first-person animations. Will cause tracking issues with animations!")]
    [Choice("Right-Handed", "Left-Handed")]
    public int FirstPersonHandednessView = 0;

    [Name("MRE Texture Variation")]
    [Description("Switches the MRE texture to a brown variant. Requires a game restart!")]
    public bool MRETextureVariant = false;
    #endregion

    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
    {
        RefreshFields();
    }

    internal void RefreshFields()
    {
        if (LightColor == FlashlightColor.Custom)
        {
            SetFieldVisible(nameof(RedValue), true);
            SetFieldVisible(nameof(GreenValue), true);
            SetFieldVisible(nameof(BlueValue), true);
        }
        else
        {
            SetFieldVisible(nameof(RedValue), false);
            SetFieldVisible(nameof(GreenValue), false);
            SetFieldVisible(nameof(BlueValue), false);
        }

        if (ExperimentalAndCheatTweaks == true)
        {
            SetFieldVisible(nameof(FirstPersonHandednessView), true);
            SetFieldVisible(nameof(MRETextureVariant), true);
            SetFieldVisible(nameof(InfiniteBattery), true);

            SetFieldVisible(nameof(FlashlightLowBeamDuration), true);
            SetFieldVisible(nameof(FlashlightHighBeamDuration), true);
            SetFieldVisible(nameof(FlashlightRechargeTime), true);
            SetFieldVisible(nameof(MinersFlashlightLowBeamDuration), true);
            SetFieldVisible(nameof(MinersFlashlightHighBeamDuration), true);
            SetFieldVisible(nameof(MinersFlashlightRechargeTime), true);
        }
        else
        {
            SetFieldVisible(nameof(FirstPersonHandednessView), false);
            SetFieldVisible(nameof(MRETextureVariant), false);
            SetFieldVisible(nameof(InfiniteBattery), false);

            SetFieldVisible(nameof(FlashlightLowBeamDuration), false);
            SetFieldVisible(nameof(FlashlightHighBeamDuration), false);
            SetFieldVisible(nameof(FlashlightRechargeTime), false);
            SetFieldVisible(nameof(MinersFlashlightLowBeamDuration), false);
            SetFieldVisible(nameof(MinersFlashlightHighBeamDuration), false);
            SetFieldVisible(nameof(MinersFlashlightRechargeTime), false);
        }
    }

    internal static void OnLoad()
    {
        Instance.AddToModSettings(BuildInfo.GUIName);
        Instance.RefreshFields();
        Instance.RefreshGUI();
    }
}