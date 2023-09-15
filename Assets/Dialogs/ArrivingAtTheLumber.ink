VAR DialogueID = "002_ArrivingAtTheLumber"

As you aproach the house of the lumberjack, you notice few people standing near the door.
"Andrey open the door, We know you're there!"
You can notice that the men are armed and they don't look too friendly.
As you aproach them, they notice you.
"Well, well who do we have here? Are you lost kiddo?"
*"I'm looking for the lumberjack, and I think you know where he is."
->Asked
*"I'm not looking for trouble."
->Trouble
*Pull out your weapon.
->Fight

===Asked===
"You look like you do have something in your pouch. And I think we're gonna take it. Get'em boys." #fight:002_LumbererEncounter
-> END
===Trouble===
"Well I think the trouble has found you. Get'em boys." #fight:002_LumbererEncounter
-> END
===Fight===
"If you're looking for a fight... I think we can deliver." #fight:002_LumbererEncounter
-> END