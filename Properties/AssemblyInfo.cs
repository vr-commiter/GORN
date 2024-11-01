using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(GORN_TrueGear.BuildInfo.Description)]
[assembly: AssemblyDescription(GORN_TrueGear.BuildInfo.Description)]
[assembly: AssemblyCompany(GORN_TrueGear.BuildInfo.Company)]
[assembly: AssemblyProduct(GORN_TrueGear.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + GORN_TrueGear.BuildInfo.Author)]
[assembly: AssemblyTrademark(GORN_TrueGear.BuildInfo.Company)]
[assembly: AssemblyVersion(GORN_TrueGear.BuildInfo.Version)]
[assembly: AssemblyFileVersion(GORN_TrueGear.BuildInfo.Version)]
[assembly: MelonInfo(typeof(GORN_TrueGear.GORN_TrueGear), GORN_TrueGear.BuildInfo.Name, GORN_TrueGear.BuildInfo.Version, GORN_TrueGear.BuildInfo.Author, GORN_TrueGear.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]