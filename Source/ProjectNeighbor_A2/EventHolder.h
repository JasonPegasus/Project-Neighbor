// EventHolder.h
#pragma once    

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "EventHolder.generated.h"     

DECLARE_DYNAMIC_DELEGATE(FSimpleEvent);                        
DECLARE_DYNAMIC_DELEGATE_OneParam(FEventWithInt, int32, Value);       

UCLASS(BlueprintType, Blueprintable)     
class PROJECTNEIGHBOR_A2_API UEventHolder : public UObject                             
{
    GENERATED_BODY()      

public:
    UFUNCTION(BlueprintCallable, Category = "Events")
    void SetEvent(const FSimpleEvent& InEvent);              

    UFUNCTION(BlueprintCallable, Category = "Events")
    void Call();

    UFUNCTION(BlueprintPure, Category = "Events")
    bool IsBound() const;

    UFUNCTION(BlueprintCallable, Category = "Events")
    void Clear();

    UFUNCTION(BlueprintCallable, Category = "Events")
    FName GetEventName() const;

    UFUNCTION(BlueprintCallable, Category = "Events", meta = (DefaultToSelf = "Outer"))
    static UEventHolder* CreateEventHolder(UObject* Outer, const FSimpleEvent& InEvent);

private:
    UPROPERTY()
    FSimpleEvent StoredEvent;
};