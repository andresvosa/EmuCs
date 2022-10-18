using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing; // For color selection
using Demo3D.Native;
using Demo3D.Visuals;

[Auto] public class BoxScript : NativeObject {
    public BoxScript(Visual sender) : base(sender) {}
    
    [Auto] CustomPropertyValue<double> WeightReading;
    //[Auto] SimplePropertyValue<string> Test;
    [Auto] SimplePropertyValue<double[]> PastReadings;
    
    double total_mass;
    
    [Auto] void OnReset(Visual sender) {
        WeightReading.ReadOnly = true;
        WeightReading.Value = 0.0;
        total_mass = 0.0;
        PastReadings.Value = new double[] {0.0, 0.0, 0.0, 0.0};
    }
     
    [Auto] void OnBlocked(PhysicsObject weight_sensor, PhysicsObject load) {
        //WeightReading.Value = load.Mass;
        total_mass += load.Mass;
        print(total_mass.ToString());
        change_color(weight_sensor);
        weight_loads(weight_sensor);
    }     
    
    [Auto] void OnCleared(PhysicsObject weight_sensor, Visual load) {
         WeightReading.Value = 0.0;
         change_color(weight_sensor);
         weight_loads(weight_sensor);
    }    
    
    void say_hello(string message){
        print("Tere");
        print(message);
    }
    
    void weight_loads(PhysicsObject weight_sensor){
        var total_weight = 0.0;
        foreach(PhysicsObject load in weight_sensor.BlockingLoads){
            total_weight += load.Mass;
        }
        for(var i = 3; i > 0; --i){
            PastReadings.Value[i] = PastReadings.Value[i - 1];
        } 
        PastReadings.Value[0] = total_weight;
        WeightReading.Value = total_weight;
        app.RefreshPropertiesGrid();
    }
    
    void change_color(PhysicsObject weight_sensor){
        int number_of_loads = weight_sensor.BlockingLoads.Count;
        if(number_of_loads == 0){
            weight_sensor.FadeToColor(Color.Green, 1);
        }
        else if(number_of_loads == 2){
            weight_sensor.FadeToColor(Color.Yellow, 1);
        }
        else{
            weight_sensor.FadeToColor(Color.Red, 1);
        }
    }
}