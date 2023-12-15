VAR outcome = 0
VAR DialogueID = "TavernTutorialPostFight"
->PostFight
===PostFight===
{
-outcome == 0:
->afterthefight
-else
->fightwithtwist
}

=== fightwithtwist  ===
You finish the fight with your unexpected ally. He looks happy but tired. You see him walking towards the counter, ordering another round of ale.
->afterthefight

=== afterthefight ===
Sitting near the counter, the barkeep gives you a bowl of stew.
"It's on the house. Eat; you'll feel better." #speaker:Innkeeper
As you eat, your hangover subsides. #speaker: 
After finishing, a blonde man sits next to you.
"Tough fight, wasn't it?" #speaker:Gary
+"They weren't too tough." #speaker: 
->FriendlyResponse
+"I know how to take care of myself." #speaker: 
->FriendlyResponse
+"I'm lucky they were drunk." #speaker: 
->FriendlyResponse
+Ignore him.
->FriendlyResponse

=== FriendlyResponse ===
"Anyway, they're dealt with. By the way, my name is Gary. Haven't seen you around. Are you from here? I can show you around." #speaker:Gary
You both get up and leave the tavern.
->END