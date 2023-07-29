Abilities mod for KTANE

[Manual](https://wouter17.github.io/Abilities/Manual/HTML/Abilities.html)

## Updating (for development)
To update the icons use the assets made available by Riot Games at https://developer.riotgames.com/docs/lol#data-dragon.

For what abilities to include use this guideline:
- Do not include passives
- Include transformation abilities (Rek'Sai, Nidalee, Elise)
- Include empowered abilities if their name and icon significantly changes (Heimerdinger, Karma)
- Do not include abilities where the name change is dependent on time (Quinn's Skystrike, Riven's Wind Slash, Yorick's Awakening, and Swain's Demonflare)
- The exception to the above rule is where the ability changing is very recognisable and should always be added to this list: (Lee Sin<Q,E>)

To convert the files used by unity to names to be used by the manual you may want to use this command:
```bash
ls --quoting-style={escape,shell,c} --ignore="*.meta" -l -m | sed 's/\∕\//g'  | sed 's/\.png//g'
```

## Legal
Abilities was created under Riot Games' ["Legal Jibber Jabber" policy](https://www.riotgames.com/en/legal) using assets owned by Riot Games.  
Riot Games does not endorse or sponsor this project.
