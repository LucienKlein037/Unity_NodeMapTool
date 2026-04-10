namespace LucienKlein
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapNodeType_Town : MapNodeType
    {
        public MapNodeType_Town(int row) : base(row)
        {
        }


        public override void Process()
        {
            Debug.Log("挑选招募队员"+"HeroTeam");
        }


        //HeroTeam
        protected override void Init()
        {
            //HeroTeam=HeroTeamFactory.Instance.GetNewHeroTeam(_row);
        }

    }

}
