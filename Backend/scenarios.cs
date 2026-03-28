namespace SpyGame {
    record Character(string Name, string Role);

    record Scenario(string Title, string Setting, Character[] Characters, string[] PlayerVisibleFacts, string[] HiddenTruths, string[] AIKnowledge);

    static class Scenarios {
        public static readonly Scenario[] All = {
            // ---- Scenario 1: The Warehouse Handoff ---------------------------------------------
            new Scenario(
                Title: "The Warehouse Handoff",
                Setting: "A late-night operation. Two locations. Three names. 9:00 PM.",
                Characters: new[] {
                    new Character("Barbara", "Handler"),
                    new Character("John", "Your cover identity - decoy operative"),
                    new Character("Davis", "Dead drop coordinator"),
                },

                // What the player sees on their card
                PlayerVisibleFacts: new[] {
                    "You are posing as John.",
                    "Names involved: Barbara, John, Davis.",
                    "Locations: Warehouse, Train Station.",
                    "Time: 9:00 PM.",
                    "No relationships have been given to you.",
                },

                // Hidden truths - shown to player on card, NEVER sent to AI
                HiddenTruths: new[] {
                    "Barbara is the handler - she is an authority figure.",
                    "The real meeting was at the warehouse at 9:00 PM.",
                    "The Train Station was a false/decoy metting location.",
                    "John (you) was expected to go to the station not the warehouse.",
                    "Davis never appears in person. He only does indirect contact.",
                    "Barbara never meets in public places.",
                },

                // What the AI knows: Scenario surface only, no truths
                AIKnowledge: new[] {
                    "Names involved: Barbara, John, Davis.",
                    "Locations mentioned: Warehouse, Train Station.",
                    "Time of operation: 9:00 PM.",
                    "No relationships have been confirmed.",
                }
            ),

            // ---- Scenario 2: The Embassy Leak -------------------------------------------
            new Scenario (
                Title: "The Embassy Leak",
                Setting: "A sensitive leak from inside an embassy. Three people. Two locations. Afternoon.",
                Characters: new[] {
                    new Character("Elena", "Double agent"),
                    new Character("Marcus", "Security officer"),
                    new Character("Savan", "Informant"),
                },

                PlayerVisibleFacts: new[] {
                    "Names involved: Elena, Marcus, Chen.",
                    "Locations: Embassy, Hotel",
                    "Time: Afternoon",
                    "No relationships have been given to you.",
                },

                HiddenTruths: new[] {
                    "Elena is a double agent. She is the central link.",
                    "Marcus is the security officer who trusts Elena.",
                    "Savan is the informant. He only communicates through Elena, never directly.",
                    "The leak happened at the Embassy during the afternoon.",
                    "The player was seen leaving the Hotel, not the Embassy.",
                    "To explain involvement, the player must use Elena as the link not claim direct content with Savan.",                    
                },

                AIKnowledge: new[] {
                    "Names involved: Elena, Marcus, Savan",
                    "Locations: Embassy, Hotel",
                    "Time: Afternoon",
                    "A leak occurred. Someone was seen leaving the hotel.",
                }
            ),

            // -------- Scenario 3: The Failed Extraction ------------------------------------------
            new Scenario (
                Title: "The Failed Extraction",
                Setting: "An extraction gone wrong. Three operatives. Two locations. Midnight and Dawn.",
                Characters: new[] {
                    new Character("Riley", "Pilot"),
                    new Character("Novak", "Field agent"),
                    new Character("Santos", "Extraction target"),
                },

                PlayerVisibleFacts: new[] {
                    "Names involved: Riley, Novak, Santos",
                    "Locations: Airfield, Safehouse",
                    "Time: Midnight, Dawn",
                    "No relationships have been given to you.",
                },

                HiddenTruths: new[] {
                    "Riley is the pilot. He arrived at the airfield at dawn as planned.",
                    "Novak is the field agent. He moved Santos early, breaking the plan.",
                    "Santos is the extraction target.",
                    "Official plan: Santos -> Safehouse -> Airfield -> extraction at dawn.",
                    "What actually happened: Novak moved Santos early.",
                    "The player encountered Santos at midnight, which was too early.",
                    "To survive: blame the deviation on Novak and stay consistent about the timeline.",
                    "Do NOT claim to have been at both the safehouse at midnight AND the airfield at dawn.",
                },

                AIKnowledge: new[] {
                    "Names involved: Riley, Novak, Santos",
                    "Locations: Airfield, Safehouse",
                    "Times involved: Midnight and Dawn",
                    "The extraction didn't go as planned.",
                }
            ),
        };
    }
}
