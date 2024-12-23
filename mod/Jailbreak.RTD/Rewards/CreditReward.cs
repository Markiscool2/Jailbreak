﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using GangsAPI.Data;
using GangsAPI.Services;
using Jailbreak.Public;
using Jailbreak.Public.Mod.RTD;
using Microsoft.Extensions.DependencyInjection;

namespace Jailbreak.RTD.Rewards;

public class CreditReward(int credits) : IRTDReward {
  public static readonly string PREFIX =
    $" {ChatColors.Purple}[{ChatColors.LightPurple}RTD{ChatColors.Purple}]";

  public string Name => $"{credits} credit{(credits == 1 ? "" : "s")}";

  public string Description => $"You won {Name}{(credits > 500 ? "!" : ".")}";

  public bool Enabled => API.Gangs != null;

  public bool PrepareReward(CCSPlayerController player) {
    var eco = API.Gangs?.Services.GetService<IEcoManager>();
    if (eco == null) return false;
    var wrapper = new PlayerWrapper(player);
    Task.Run(async () => await eco.Grant(wrapper, credits, true, "RTD"));

    if (Math.Abs(credits) >= 5000)
      Server.PrintToChatAll(
        $"{PREFIX} {ChatColors.Yellow}{wrapper.Name} {ChatColors.Default}won the jackpot of {ChatColors.Green}{credits} {ChatColors.Default}credits!");

    return true;
  }

  public bool GrantReward(CCSPlayerController player) { return true; }
}