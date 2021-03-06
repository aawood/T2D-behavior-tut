//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function DeadlyReef::create( %this )
{
    // We need a main "Scene" we can use as our game world.  The place where sceneObjects play.
    // Give it a global name "mainScene" since we may want to access it directly in our scripts.
    new Scene(mainScene);

    // Without a system window or "Canvas", we can't see or interact with our scene.
    // AppCore initialized the Canvas already

    // Now that we have a Canvas, we need a viewport into the scene.
    // Give it a global name "mainWindow" since we may want to access it directly in our scripts.
    new SceneWindow(mainWindow);
    mainWindow.profile = new GuiControlProfile();
    Canvas.setContent(mainWindow);

    // Finally, connect our scene into the viewport (or sceneWindow).
    // Note that a viewport comes with a camera built-in.
    mainWindow.setScene(mainScene);
    mainWindow.setCameraPosition( 0, 0 );
    mainWindow.setCameraSize( 100, 75 );

    // load some scripts and variables
    // exec("./scripts/someScript.cs");
    exec("./scripts/aquarium.cs");
	exec("./scripts/behaviors/movement/shooterControls.cs");
	exec("./scripts/behaviors/movement/drift.cs");
	exec("./scripts/behaviors/movement/meander.cs");
	exec("./scripts/behaviors/life/lifeTimer.cs");

    buildAquarium(mainScene);
    createAquariumEffects(mainScene);

    DeadlyReef.spawnPlayerFish();
	
	for (%i = 0; %i < 2; %i++)
		DeadlyReef.spawnFishFood();
	
	for (%i = 0; %i < 5; %i++)
		DeadlyReef.spawnEnemyFish();
}

//-----------------------------------------------------------------------------

function DeadlyReef::destroy( %this )
{
}

//-----------------------------------------------------------------------------

function DeadlyReef::spawnPlayerFish(%this)
{
    %anim = "TropicalAssets:seahorseAnim";    
    %size = getFishSize(%anim);

    %fish = new Sprite()
    {
        Animation = %anim;
        // class = "FishClass";
        position = "0 0";
        size = %size;
        SceneLayer = "15";
        SceneGroup = "14";
        minSpeed = "5";
        maxSpeed = "15";
        CollisionCallback = true;
    };

    %fish.createPolygonBoxCollisionShape(%size);
    %fish.setCollisionShapeIsSensor(0, true);
    %fish.setCollisionGroups( "10 15" );

	%controls = ShooterControlsBehavior.createInstance();
	%controls.upKey = "keyboard W";
	%controls.downKey = "keyboard S";
	%controls.leftKey = "keyboard A";
	%controls.rightKey = "keyboard D";
	%fish.addBehavior(%controls);
	
	%life = LifeTimerBehavior.createInstance();
	%fish.addBehavior(%life);
	
    mainScene.add( %fish ); 
}

function DeadlyReef::spawnFishFood()
{

	%food = new Sprite()
	{
		image = "TropicalAssets:bubble";
		class = "FishFoodClass";
		position = "20 20";
		size = "3 3";
		SceneLayer = "15";
		SceneGroup = "10";
		CollisionCallback = true;
	};

	%food.createPolygonBoxCollisionShape(5, 5);
	%food.setCollisionShapeIsSensor(0, true);
	%food.setCollisionGroups(15);
	
	%move = DriftBehavior.createInstance();
	%food.addBehavior(%move);
	
	mainScene.add(%food);
	%food.nutrition = 10;
}

function DeadlyReef::spawnEnemyFish(%this)
{
	%position = getRandom(-55, 55) SPC getRandom(-20, 20);
	%index = getRandom(0, 5);
	%anim = getUnit(getFishAnimationList(), %index, ",");
	
	%fishSize = getFishSize(%anim);
	
	%fish = new Sprite()
	{
		Animation = %anim;
		class = "NPCFishClass";
		position = %position;
		size = %fishSize;
		SceneLayer = "15";
		SceneGroup = "14";
		CollisionCallback = true;
	};
	
	%fish.createPolygonBoxCollisionShape(%fishSize);
	%fish.setCollisionShapeIsSensor(0, true);
	%fish.setCollisionGroups("10 15");
	%fish.fixedAngle = true;
	
	%move = MeanderBehavior.createInstance();
	%fish.addBehavior(%move);
	
	mainScene.add(%fish);
}