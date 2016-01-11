using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using System;


public class Enemy : MonoBehaviour
{
    private LinkedList<Vector3> path;

    public float speed;

    internal void setPath(LinkedList<MazeCell> startPath)
    {
        path = new LinkedList<Vector3>();
        foreach(MazeCell mc in startPath)
        {
            path.AddLast(mc.gameObject.transform.localPosition);
        }
        
    }

    public void brodcastPosition(Vector3 vector3)
    {
        lock(path)
        {
            if (path.Last.Value.Equals(vector3))
            {
                return;
            }

            path.AddLast(vector3);
        }

    }

    public void Update()
    {
        lock(path)
        {
            if (path.Count < 2)
            {
                return;
            }
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            Vector3 newPosition = path.First.Next.Value;

           
            Vector3 move = (newPosition - currentPosition);
            
            this.GetComponent<Rigidbody>().AddForce(move * speed);

            if (move.magnitude < 0.2f)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                path.RemoveFirst();
            }
        }
    }



    public void OnTriggerEnter(Collider col)
    {
       /* Debug.Log("Collided with new MazeCell " + col.gameObject.transform.localPosition);
        MazeCell newCell = col.gameObject.GetComponent<MazeCell>();
        if(newCell != null && path.Contains(newCell))
        {
            var current = path.First;
            while(current != path.Last && current.Value != newCell)
            {
                path.Remove(current);
                current = path.First;
            }
            
        }
        else
        {
            path.AddFirst(newCell);
        }
        Debug.Log("Curr: " + newCell.gameObject.transform.localPosition + " next " + path.First.Value.gameObject.transform.localPosition);*/
    }

   


}