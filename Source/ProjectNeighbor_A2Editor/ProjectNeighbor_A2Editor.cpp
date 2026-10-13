#include "ProjectNeighbor_A2Editor.h"
#include "Styling/SlateStyle.h"
#include "Styling/SlateStyleRegistry.h"
#include "Misc/Paths.h"
#include "HAL/FileManager.h"

IMPLEMENT_MODULE(FProjectNeighbor_A2EditorModule, ProjectNeighbor_A2Editor)

void FProjectNeighbor_A2EditorModule::StartupModule()
{
    StyleSet = MakeShareable(new FSlateStyleSet("ProjectNeighborStyle"));

    const FString IconsDir = FPaths::ProjectDir() / TEXT("CustomResources/ClassIcons");

    // Busca PNGs en la carpeta y en sus subcarpetas
    TArray<FString> Files;
    IFileManager::Get().FindFilesRecursive(Files, *IconsDir, TEXT("*.png"), true, false);

    for (const FString& FilePath : Files)
    {
        const FString ClassName = FPaths::GetBaseFilename(FilePath); // "BP_Item_C"
        const FName BrushName(*FString::Printf(TEXT("ClassIcon.%s_C"), *ClassName));


        StyleSet->Set(FName(*FString::Printf(TEXT("ClassThumbnail.%s_C"), *ClassName)), new FSlateImageBrush(FilePath, FVector2D(64, 64)));
        StyleSet->Set(BrushName, new FSlateImageBrush(FilePath, FVector2D(16, 16)));
    }

    FSlateStyleRegistry::RegisterSlateStyle(*StyleSet);
}

void FProjectNeighbor_A2EditorModule::ShutdownModule()
{
    FSlateStyleRegistry::UnRegisterSlateStyle(*StyleSet);
    StyleSet.Reset();
}