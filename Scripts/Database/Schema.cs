using SimpleSQL;
using System;

namespace Studio.Data {




// ------------------- SYSTEM DB TABLES -------------------
public class Settings { // Universal Settings
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    public string LastProject { get; set; }             // The last project name that was opened

    // More to come...

    [NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}

public class Tags { // Universal Tag System shared across All Projects
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    public string Tag { get; set; }             // The actual name of the Tag
    public string Color { get; set; }           // Hex Color, e.g. #FF0000, if empty, use default color
    public int UsageCount { get; set; }         // How many times has this tag been used?
    public string Description { get; set; }     // (Optional) Description of the Tag
    public string Category { get; set; }        // (Optional) Category of the Tag

    [NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}

public class TagLinks { // Links Tags to Items in Projects
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    public string ProjectName { get; set; } 	// The Project Name the Tag is linked to
	public string ItemTable { get; set; } 	    // The Table Name the Tag is linked to
	public int ItemID { get; set; } 		    // The unique row in that Table
	public int TagID { get; set; } 			    // The Tag's unique ID

    [NotNull] public System.DateTime Updated { get; set; }
    [NotNull] public System.DateTime Created { get; set; }
}










// ------------------- PROJECT DB TABLES (Content) -------------------

public class Media {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
	public int Type { get; set; }              // 0 - Image (jpg, png), 1 - Video (mp4)  |  Audio:   2 - Sound, 3 - Music, 4 - Ambient, 5 - Narration, 6 - Voice  (mp3, wav)
	public string Name { get; set; }           // Name of the Media
	public string URL { get; set; } 		   // Local Media URL - 2025/03/15/mediafile001.mp4
	public string Thumb { get; set; } 		   // Local Thumbnail Image URL - 2025/03/15/Thumbs/mediafile001_thumb.jpg

	[NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}

public class AIPrompts {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
	public int SceneBlockID { get; set; }
	public int AIType { get; set; }			// 0 - Text, 1 - Image, 2 - Video, 3 - Music, 4 - Ambient, 5 - Sounds

	public string Name { get; set; } 		// Short Name for the Prompt as an Identifier
	public string Prompt { get; set; } 		// The Prompt itself
	public string Notes { get; set; } 		// (Optional) Any Notes for devs related to the Prompt

	[NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}



//===========================================================================
public class Characters {  // Basic Character Information - only this is needed for character search and selection
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
	public int DefaultMediaID { get; set; } 	// The MediaID of the chosen Default Image
	public int UsageCount { get; set; }         // How many times was this character used in Scenes

	public string Name { get; set; }            // Name of the Character
	public string Summary { get; set; }         // Short Character Summary

    [NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}

public class CharacterData {  // Extended Character Information - this is where all the character details are stored
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
	public int CharID { get; set; }         // The ID of the corresponding Characters


	//========== 1. Basic Information ==========
	public string Alias { get; set; }
	public string Race { get; set; }
	public string Class { get; set; }
	public string Gender { get; set; }

	public int Age { get; set; }
	public int Height { get; set; }   		// in cm
	public int Weight { get; set; }     	// in kg

	//========== 2. Looks & Voice ==========
	public string Appearance { get; set; }
	public string AttireEquipment { get; set; }
	public string VoiceSpeech { get; set; }
	public string CinematicPresence { get; set; }

	//========== 3. Prompts ==========
	public string CharacterPrompt { get; set; }
	public string ImagePrompt { get; set; }
	public string PortraitPrompt { get; set; }

	//========== 4. Physical Toll & Hardships ==========
	public string LastingInjuries { get; set; }
	public string AddictionsDependencies { get; set; }
	public string ScarsMarks { get; set; }

	//========== 5. Personality ==========
	public string CoreTraits { get; set; }
	public string Strengths { get; set; }
	public string Weaknesses { get; set; }
	public string SenseOfHumor { get; set; }
	public string MannerismsHabits { get; set; }

	//========== 6. Psychological Profile ==========
	public string FearsPhobias { get; set; }
	public string AmbitionsGoals { get; set; }
	public string PsychologicalArchetypes { get; set; }
	public string Secrets { get; set; }

	//========== 7. Mental State & Sanity ==========
	public string SanityThreshold { get; set; }
	public string Traumas { get; set; }
	public string CopingMechanisms { get; set; }
	public string Susceptibility { get; set; }

	//========== 8. Backstory & Life Events ==========
	public string Origin { get; set; }
	public string DefiningMoment { get; set; }
	public string BiggestRegret { get; set; }
	public string GreatestAchievement { get; set; }

	//========== 9. Relationships ==========
	public string NotableRelationships { get; set; }
	public string Betrayals { get; set; }
	public string TragicLosses { get; set; }
	public string DebtsBargains { get; set; }
	public string ToxicRelationships { get; set; }

	//========== 10. Underworld & Forbidden Connections ==========
	public string ShadyContacts { get; set; }
	public string ForbiddenKnowledge { get; set; }
	public string ShadowFactionTies { get; set; }

	//========== 11. Skills, Abilities & Combat Style ==========
	public string FightingStyle { get; set; }
	public string SpecialAbilities { get; set; }
	public string WeaponsTools { get; set; }
	public string TacticsStrategies { get; set; }
	public string SurvivalSkills { get; set; }

	//========== 12. Corruption / Taint ==========
	public string CorruptionSources { get; set; }
	public string CurrentCorruptionLevel { get; set; }
	public string ResistanceVulnerability { get; set; }

	//========== 13. Morals ==========
	public string MoralAlignment { get; set; }
	public string MoralCompassDetails { get; set; }
	public string LinesCrossed { get; set; }
	public string LinesTheyWontCross { get; set; }
	public string MajorMoralDilemmas { get; set; }

	//========== 14. Beliefs & Motivations ==========
	public string FaithSpirituality { get; set; }
	public string PoliticalViews { get; set; }
	public string PersonalPhilosophy { get; set; }

	//========== 15. Daily Life & Social Aspects ==========
	public string ProfessionSkills { get; set; }
	public string HobbiesInterests { get; set; }
	public string SocialSkillsCharisma { get; set; }
	public string ReputationLegacy { get; set; }
	public string FavoriteFoodsDrinks { get; set; }
	public string ResourceManagement { get; set; }

	//========== 16. Story Role & Narrative Purpose ==========
	public string WhyAreTheyImportant { get; set; }
	public string ForeshadowingSymbolism { get; set; }
	public string PotentialConflicts { get; set; }
	public string CharacterArc { get; set; }
	public string NarrativeHooks { get; set; }

	//========== 17. Notes & Additional Details ==========
	public string NotesDetails { get; set; }


    [NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}

public class CharacterLinks {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
	public string ItemTable { get; set; } 	// Table Name
	public int ItemID { get; set; } 		// The unique row in that Table
	public int CharID { get; set; } 		// The Character's unique ID

    [NotNull] public System.DateTime Updated { get; set; }
	[NotNull] public System.DateTime Created { get; set; }
}




}