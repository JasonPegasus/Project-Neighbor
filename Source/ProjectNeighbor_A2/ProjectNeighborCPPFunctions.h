// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "ProjectNeighborCPPFunctions.generated.h"

/**
 * 
 */
UCLASS()
class PROJECTNEIGHBOR_A2_API UProjectNeighborCPPFunctions : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:
	UFUNCTION(BlueprintCallable, CallInEditor, meta = (DevelopmentOnly, DisplayName = "Set Editor Icon For Actor Class"))
	static void SetEditorIconForActorClass(UObject* ActorOrClass, UTexture2D* NewIcon);

	UFUNCTION(BlueprintPure, meta = (DisplayName = "Is in Editor"))
	bool inEditor() const
	{
		#if WITH_EDITOR
				return true;
		#else
				return false;
		#endif
	}
	
};
