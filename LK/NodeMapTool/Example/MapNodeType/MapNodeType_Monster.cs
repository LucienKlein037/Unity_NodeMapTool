using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LucienKlein
{

    public class MapNodeType_Monster : MapNodeType
    {
        public MapNodeType_Monster(int row) : base(row)
        {
        }

        //Team BattleMonsterTeam;

        public override void Process()
        {
            Debug.Log("与怪物战斗"+"BattleMonsterTeam");


        }
        protected override void Init()
        {
            //BattleMonsterTeam=MonsterTeamFactory.Instance.GetNewMonsterTeam(_row);

        }

    }

}
