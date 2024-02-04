﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Jailbreak.Public.Behaviors;
using Jailbreak.Public.Extensions;
using Jailbreak.Public.Mod.Rebel;

namespace Jailbreak.Teams;

public class RebelListener : IPluginBehavior
{
    private IRebelService _rebelService;
    
    public RebelListener(IRebelService rebelService)
    {
        _rebelService = rebelService;
    }
    
    public void Start(BasePlugin parent)
    {
        parent.RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
    }

    HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!player.IsValid)
            return HookResult.Continue;
        if (player.GetTeam() != CsTeam.CounterTerrorist)
            return HookResult.Continue;

        var attacker = @event.Attacker;
        if (!attacker.IsValid)
            return HookResult.Continue;

        if (attacker.GetTeam() != CsTeam.Terrorist)
            return HookResult.Continue;

        _rebelService.MarkRebel(attacker, 120); 
        return HookResult.Continue;
    }
}