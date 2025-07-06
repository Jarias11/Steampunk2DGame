using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TaggedRuleTile", menuName = "Tiles/Tagged Rule Tile")]
public class TaggedRuleTile : RuleTile<TaggedRuleTile.Neighbor> {
    public bool isDirtType = false; // Tag for dirt or transition tiles

    public class Neighbor : TilingRule.Neighbor {
        public const int IsDirtType = 3; // Custom neighbor type
    }

    public override bool RuleMatch(int neighbor, TileBase other) {
        if (neighbor == Neighbor.IsDirtType) {
            if (other is TaggedRuleTile typedTile)
                return typedTile.isDirtType;
            return false;
        }

        return base.RuleMatch(neighbor, other);
    }
}