VAR DialogueID = "002_ArrivingAtTheLumber"
As you approach the house of the lumberjack, you notice a few people standing near the door. #speaker: 

"Andrey, open the door! We know you're there!" #speaker:Bandit_1

You can see that the men are armed, and they don't look too friendly. As you approach them, they notice you. #speaker: 

"Well, well, who do we have here? Are you lost, kiddo?" #speaker:Bandit_2

*["I'm looking for the lumberjack, and I think you know where he is."]
->Asked
*["I'm not looking for trouble."]
->Trouble
*[Pull out your weapon.]
->Fight

===Asked===
"You look like you do have something in your pouch. And I think we're gonna take it. Get 'em, boys." #fight:002_LumbererEncounter #speaker:Bandit_1
->END
===Trouble===
"Well, I think trouble has found you. Get 'em, boys." #fight:002_LumbererEncounter #speaker:Bandit_1
->END
===Fight===
"If you're looking for a fight... I think we can deliver." #fight:002_LumbererEncounter #speaker:Bandit_1
->END
