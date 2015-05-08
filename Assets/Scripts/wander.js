 var Speed= 1;
 var wayPoint : Vector3;
 var Range= 1;
 
 function Start(){
    //initialise the target way point
    Wander();
 }
 
 function Update() 
 {
    // this is called every frame
    // do move code here
    transform.position += transform.TransformDirection(Vector3.forward)*Speed*Time.deltaTime;
     if((transform.position - wayPoint).magnitude < 3)
     {
         // when the distance between us and the target is less than 3
         // create a new way point target
         Wander();
 
 
     }
     transform.rotation.z = 0;
     transform.rotation.x = 0;
 }
 

 function Wander()
 { 
    // does nothing except pick a new destination to go to
     
     wayPoint=  Vector3(Random.Range(transform.position.x - Range, transform.position.x + Range), -2, Random.Range(transform.position.z - Range, transform.position.z + Range));
     wayPoint.y = -2;
    // don't need to change direction every frame seeing as you walk in a straight line only
     transform.LookAt(wayPoint);
     Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
 }