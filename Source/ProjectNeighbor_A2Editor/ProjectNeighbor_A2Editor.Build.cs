using UnrealBuildTool;

public class ProjectNeighbor_A2Editor : ModuleRules
{
    public ProjectNeighbor_A2Editor(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine" });
        PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore", "Projects", "UnrealEd" });
    }
}