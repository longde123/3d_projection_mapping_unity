﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstantiator : MonoBehaviour {

    public GameObject[] buildingPositions;
    public GameObject[] buildingPrefabsGood;
    public GameObject[] buildingPrefabsBad;
    //public ScanningDataParser scanningDataParser;
    public ScanningSliderDataParser scanningSliderDataParser;

    public GameObject bldgParent;
    public GameObject[] bldgGOsGood = new GameObject[18];
    public GameObject[] bldgGOsBad = new GameObject[18];

    public int[] currentTypeList = new int[18];
    public int[] currentRotList = new int[18];
    public bool[] changedList = new bool[18];

    public BoxMorphKeyPts boxMorphKeyPts;

    public GameObject waveEffect;
    public float waveEffectPosY = 0.3f;

    void Start ()
    {
        /*
        // instantiate buildings
        for (int i = 0; i < scanningDataParser.grid.buildings.Count; i ++)
        {
            //Debug.Log(string.Format("i = {0}", i));
            ScanningDataParser.Building b = scanningDataParser.grid.buildings[i];
            //Debug.Log(string.Format("b = {0}", b));
            if (b.type != -1 && buildingPrefabsGood[b.type] != null)
            {
                //Debug.Log(string.Format("instantiating building {0} in the grid", i));
                bldgGOsGood[i] = Instantiate(buildingPrefabsGood[b.type], buildingPositions[i].transform.position, buildingPositions[i].transform.rotation, bldgParent.transform);
                bldgGOsBad[i] = Instantiate(buildingPrefabsBad[b.type], buildingPositions[i].transform.position, buildingPositions[i].transform.rotation, bldgParent.transform);
            }
        }
        */

        // initiate current frame values with -1
        for (int i = 0; i < 18; i++)
        {
            currentTypeList[i] = -1;
            currentRotList[i] = -1;
        }

        // WRONG! This will do reference overwrite
        //currentTypeList = scanningDataParser.grid.typeList;  
        //currentRotList = scanningDataParser.grid.rotList;
    }

    void Update ()
    {
        // compare current frame list values of type and rot with latest UDP json feed
        for (int i = 0; i < 18; i++)
        {
            int type = scanningSliderDataParser.grid.buildings[i].type;
            int rot = scanningSliderDataParser.grid.buildings[i].rot;

            if (currentTypeList[i] == type && currentRotList[i] == rot)
            {
                changedList[i] = false;
                //Debug.Log("unchanged");
            }
            else
            {
                changedList[i] = true;
                //Debug.Log("changed");
                currentTypeList[i] = type;
                currentRotList[i] = rot;

                // remove current building GO and instantiate latest one
                Destroy(bldgGOsGood[i]);
                Destroy(bldgGOsBad[i]);

                // instantiate buildings
                if (type != -1 && buildingPrefabsGood[type] != null)
                {
                    //Debug.Log(string.Format("instantiating building {0} in the grid with type {1}, rot {2}", i, type, 0));
                    bldgGOsGood[i] = Instantiate(buildingPrefabsGood[type], buildingPositions[i].transform.position, Quaternion.Euler(0.0f, -(float)rot * 90.0f + 180.0f, 0.0f), bldgParent.transform);
                    BoxMorphGO c1 = bldgGOsGood[i].AddComponent<BoxMorphGO>();
                    c1.boxMorphKeyPts = boxMorphKeyPts;

                    bldgGOsBad[i] = Instantiate(buildingPrefabsBad[type], buildingPositions[i].transform.position, Quaternion.Euler(0.0f, -(float)rot * 90.0f + 180.0f, 0.0f), bldgParent.transform);
                    BoxMorphGO c2 = bldgGOsBad[i].AddComponent<BoxMorphGO>();
                    c2.boxMorphKeyPts = boxMorphKeyPts;
                }

                // instantiate wave effect for only type 20 and 21 (pyramid and empirestate)
                if (type == 20 || type == 21)
                {
                    //Debug.Log(string.Format("instantiating wave effect in the grid with type {1}, rot {2}", i, type, 0));
                    Instantiate(waveEffect, buildingPositions[i].transform.position + new Vector3(0.0f,waveEffectPosY,0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f), bldgParent.transform);
                }
            }
        }
    }
}
