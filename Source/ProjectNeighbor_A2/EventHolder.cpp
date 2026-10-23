#include "EventHolder.h"

void UEventHolder::SetEvent(const FSimpleEvent& InEvent)                      
{ StoredEvent = InEvent; }

void UEventHolder::Call()
{ StoredEvent.ExecuteIfBound(); }

bool UEventHolder::IsBound() const
{ return StoredEvent.IsBound(); }

FName UEventHolder::GetEventName() const
{ return StoredEvent.GetFunctionName(); }

void UEventHolder::Clear()         
{ StoredEvent.Unbind(); }

UEventHolder* UEventHolder::CreateEventHolder(UObject* Outer, const FSimpleEvent& InEvent)
{
    // Si no te pasan Outer, uso el paquete transitorio como respaldo
    UObject* ActualOuter = Outer ? Outer : GetTransientPackage();

    UEventHolder* Holder = NewObject<UEventHolder>(ActualOuter);
    Holder->SetEvent(InEvent);
    return Holder;
}