VAR outcome = 0
VAR DialogueID = "TavernTutorialPostFight"

-> PostFight
=== PostFight ==
    {
        - outcome == 0:
            -> afterthefight
        - else:
            -> fightwithtwist
    }

=== fightwithtwist  ===
You finish the fight with your unexpected ally. He looks happy but tired. You see him walking towards the counter ordering another round of ale.
->afterthefight

=== afterthefight ===
After the fight you sit down near the counter. The barkeep gives you a bowl of stew.
"It's on the house. Eat you'll feel better."
As you start to eat you feel your hangover giving up.
Soon after you finish your bowl, you see a blonde man sitting right next to you.
"Tought fight wasn't it?"
+"They weren't too tought."
->FriendlyResponse
+"I know how to take care of myself."
->FriendlyResponse
+"I'm lucky they were drunk."
->FriendlyResponse
+Ignore him.
->FriendlyResponse

=== FriendlyResponse ===
"Anyway they're dealt with. By the way, my name is Gary. I haven't seen you around. Are you from here? I can show you around."
You both get up from your chairs and you leave the Tavern.
->END

