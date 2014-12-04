using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ObstaclesData
{
    public Dictionary<CollisionType, ObstacleData> Data { get; private set; }

    public ObstaclesData()
    {
        Data = new Dictionary<CollisionType, ObstacleData>();
        
        ListFeed list = GDriveManager.GetSpreadsheet(WhoaPlayerProperties.DRIVE_DOCUMENT_URL, 5);
        foreach (ListEntry row in list.Entries)
        {
            string name = row.Elements[0].Value;
            int count = int.Parse(row.Elements[1].Value);
            float addedPerLevel = float.Parse(row.Elements[2].Value);
            float xVelocityMax = float.Parse(row.Elements[3].Value);
            float xVelocityMin = float.Parse(row.Elements[4].Value);
            float yVelocityMax = float.Parse(row.Elements[5].Value);
            float yVelocityMin = float.Parse(row.Elements[6].Value);
            float offset  = float.Parse(row.Elements[7].Value);
            int damage = int.Parse(row.Elements[8].Value);
            float mass = float.Parse(row.Elements[9].Value);
            Data.Add((CollisionType)Enum.Parse(typeof(CollisionType), name), new ObstacleData(count, addedPerLevel, xVelocityMin, xVelocityMax, yVelocityMin, yVelocityMax, offset, damage, mass));
        }
    }
}

