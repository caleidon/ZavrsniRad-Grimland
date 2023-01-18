using BehaviorDesigner.Runtime;
using JetBrains.Annotations;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

public class HumanDef : CreatureDef
{
    [LoadedByYaml(YamlTag = "!Human")]
    public class Initializer : IInitializable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Behavior { get; set; }
        public string Sprite { get; set; }
        public float BaseMovementSpeed { get; set; }

        public object CreateInstance()
        {
            HumanDef humanDef = new HumanDef
            {
                Name = Name,
                DisplayName = DisplayName,
                Behavior = Behavior,
                Sprite = Sprite,
                BaseMovementSpeed = BaseMovementSpeed
            };

            return humanDef;
        }
    }

    public override Creature SpawnCreature(Vector3Int node)
    {
        Human human = new Human(null, this)
        {
            CreatureGO = AssembleCreatureGO()
        };

        human.SetLocation(node);
        human.InitializeBehaviorTree();

        return human;
    }

    // TODO: these methods are creature, and not human specific
    public override GameObject AssembleCreatureGO()
    {
        GameObject humanGO = new GameObject();
        humanGO.name = "Human";
        // TODO: set gameobject name equal to actual name of human


        var spriteRenderer = humanGO.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = DefManager.Sprites[Sprite];
        spriteRenderer.sortingOrder = (int)Settings.LayerOrder.Creature;

        var seeker = humanGO.AddComponent<Seeker>();

        var aiPath = humanGO.AddComponent<AIPath>();
        aiPath.maxSpeed = BaseMovementSpeed;
        aiPath.orientation = OrientationMode.YAxisForward;
        aiPath.gravity = Vector3.zero;
        aiPath.enableRotation = false;
        aiPath.radius = 0.25f;
        aiPath.pickNextWaypointDist = 0.75f;
        aiPath.slowdownDistance = 0.75f;
        aiPath.endReachedDistance = 0.15f;
        aiPath.constrainInsideGraph = true;
        aiPath.autoRepath = new AutoRepathPolicy
        {
            mode = AutoRepathPolicy.Mode.Never
        };
        aiPath.rvoDensityBehavior = new RVODestinationCrowdedBehavior
        {
            enabled = true,
            densityThreshold = 0.5f,
            returnAfterBeingPushedAway = true
        };


        var rvoController = humanGO.AddComponent<RVOController>();
        rvoController.lockWhenNotMoving = false;
        rvoController.maxNeighbours = 30;
        // TODO: check these collision settings
        // rvoController.collidesWith = RVOLayer.Layer2;

        var mover = humanGO.AddComponent<Mover>();

        var simpleSmooth = humanGO.AddComponent<SimpleSmoothModifier>();
        simpleSmooth.uniformLength = true;
        simpleSmooth.maxSegmentLength = 0.3f;
        simpleSmooth.iterations = 5;
        simpleSmooth.strength = 1;

        var bt = humanGO.AddComponent<BehaviorTree>();
        bt.StartWhenEnabled = true;
        bt.BehaviorName = "Human Behavior";
        bt.ExternalBehavior = DefManager.Behaviors[Behavior];

        return humanGO;
    }
}

public class Human : Creature
{
    public Human([CanBeNull] string id, ThingDef thingDef) : base(id, thingDef) { }
}