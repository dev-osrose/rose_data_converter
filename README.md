# rose_data_converter

Exports the game data of a ROSE Online client (`3DDATA`) to JSON for the
servers.

```sh
cd rose_data/bin/Debug/net10.0
dotnet rose_data.dll --root <directory containing 3DDATA>
```

The files are written to `srv_data/` in the current directory. Paths inside the
game data are matched against the disk without regard to case, so an all
upper-case or all lower-case copy of the data works.

It reads the game's files with the dev-osrose fork of
[Revise](https://github.com/dev-osrose/Revise), expected at
`~/git/raven/Revise` (pass `-p:ReviseProject=<path to Revise.csproj>` to
`dotnet build` to use another checkout).

## Exports

| File | From | What |
|---|---|---|
| `item_db.json` | `LIST_*.STB` | every item, by `Type` and `Id`; row 0 of each table is "nothing" (bare hands, bare feet). Weapons have `MotionType`, the column of `motion_types.json` for their motions. |
| `npc_db.json` | `LIST_NPC.STB` | NPCs and monsters: stats, speeds, `AiType` (an id of `ai_db.json`), drop columns |
| `skill_db.json` | `LIST_SKILL.STB` | skills |
| `zone_data.json` | `LIST_ZONE.STB`, `.ZON`, `.IFO`, `WARP.STB` | per zone: NPCs, monster regen points, start/respawn points, warp gates (`WarpPoints`: the gate's box, its `WarpId` (what the client sends in `PAKCS_TELEPORT_REQ`), the destination map in `DestinationGateId` and position in `Destination`; a gate whose destination cannot be found is left out with a message) |
| `motion_ref.json` | `FILE_MOTION.STB`, `.ZMO` | avatar motions by id: frame count, frames per second, frame events and how many of them are attack hits |
| `motion_types.json` | `TYPE_MOTION.STB` | the avatar motion for an action with a weapon: `Motions[MotionType]` of the action's row (`FILE_MOTION(weapon, action)`; action 8 is attack; motion type 0 is bare hands) |
| `npc_motions.json` | `NPC/LIST_NPC.CHR`, `.ZMO` | the motions of each NPC and monster by `Type` (Stop, Move, Attack, Hit, Die, ...), with their timing and hit frames |
| `ai_db.json` | `FILE_AI.STB`, `AI/*.AIP` | monster AI scripts by `AiType`: per event (`Created`, `Stop`, `AttackMove`, `Damaged`, `Kill`, `Dead`) the rules tried in order, each a list of conditions and actions. Distances are in metres as in the files. |
| `ai_list.json` | `FILE_AI.STB` | the AI file names alone, by `AiType` |
| `quest_db.json` | `LIST_QUESTDATA.STB`, `QUESTDATA/*.QSD` | quest scripts: per file, patterns of triggers by `Name`, each with the conditions that all have to hold and the rewards then given; `CheckNext` tries the pattern's next trigger when one fails. Every record has its `Type` (`STR_COND_`/`STR_REWD_` of the game's `IO_Quest.h`) and that structure's fields. |
| `skill_points.json` | `LIST_SKILL_P.STB` | skill points gained on reaching a level |
| `grade_db.json` | `LIST_GRADE.STB` | what each refine grade adds (attack, hit, defense, resistance, avoid) |
| `status_db.json` | `LIST_STATUS.STB` | lasting states: `Type` (1 HP over time, 2 MP over time, ...), `CanDuplicate`, and `Apply`, the states each applies with its value (for potions, per second). A consumable's `StatusId` in `item_db.json` names the row it starts. |
| `store_db.json` | `LIST_SELL.STB` | what NPC stores sell, by sell tab (`SellTab0`..`3` of `npc_db.json`); `Values[c]` is the game's column `c`, so its `STORE_ITEM(row, n)` is `Values[2 + n]`, an item as `type * 1000 + id` |
| `drop_db.json` | `ITEM_DROP.STB` | drop tables by drop type or zone; `Values[c]` is the game's column `c`, so its `DROPITEM_ITEMNO(row, n)` is `Values[1 + n]` |
