using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NaughtyAttributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = System.Random;

public class PathfindingNode
{
    public bool IsSolid {get; set;}
    public bool Explored {get; set;}
    public int X {get; set;}
    public int Y {get; set;}
    public int Z {get; set;}
    public PathfindingNode Previous {
        get => previous; 
        set {
            previous = value;
            if(previous != null){
                G = value.G + 1;
            }
            
        } 
    }
    private PathfindingNode previous;
    public float H {get; set;}
    public float G {get; private set;}
    public float F => H + G;

    public PathfindingNode(bool isSolid, bool explored, int x, int y, int z, PathfindingNode previous, int h){
        this.IsSolid = isSolid;
        this.Explored = explored;
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Previous = previous;
        this.H = h;
    }

    public static PathfindingNode StartNode(int x, int y, int z, int h){
        var node = new PathfindingNode(false, true, x,y,z, null, h);
        node.G = 0;
        return node;
    }

    public static PathfindingNode Solid(int x, int y, int z){
        var node = new PathfindingNode(true, false, x, y, z, null, int.MaxValue);
        return node;
    }
    public PathfindingNode(){
    }
}