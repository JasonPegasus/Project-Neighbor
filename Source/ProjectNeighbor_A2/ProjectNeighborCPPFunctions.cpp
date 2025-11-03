#include "ProjectNeighborCPPFunctions.h"

#if WITH_EDITOR
#include "Engine/BlueprintGeneratedClass.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Editor.h"
#endif

void UProjectNeighborCPPFunctions::SetEditorIconForActorClass(UObject* ActorOrClass, UTexture2D* NewIcon)
{
	#if WITH_EDITOR
	if (!ActorOrClass || !NewIcon) return;

		UClass* TargetClass = nullptr;
		if (AActor* Actor = Cast<AActor>(ActorOrClass))
		{
			TargetClass = Actor->GetClass();
		}
		else if (UClass* Class = Cast<UClass>(ActorOrClass))
		{
			TargetClass = Class;
		}
		if (!TargetClass)return;

		if (UBlueprintGeneratedClass* BPClass = Cast<UBlueprintGeneratedClass>(TargetClass))
		{
			if (UBlueprint* BP = Cast<UBlueprint>(BPClass->ClassGeneratedBy))
			{
				
			}
		}

	#endif
}