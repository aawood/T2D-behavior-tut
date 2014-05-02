if( !isObject(MeanderBehavior))
{
	%template = new BehaviorTemplate(MeanderBehavior);
	%template.friendlyName = "Meander";
	%template.behaviorType = "Movement";
	%template.description = "Meander from left to right. Provide a Recycle feature";
	%template.addBehaviorField(minSpeed, "Minimum speed to meander", float, 5.0);
	%template.addBehaviorField(maxSpeed, "Maximum speed to meander", float, 15.0);
}

function MeanderBehavior::onCollision(%this, %object, %collisionDetails)
{
	if (%object.class $= "AquariumBoundary")
	{
		%this.recycle(%object.side);
	}
	else if (%object.class $= "FishFoodClass")
	{
		%object.recycle();
	}
}

function MeanderBehavior::onBehaviourAdd(%this)
{
	%speed = getRandom(%this.minSpeed, %this.maxSpeed);
	
	if (getRandom(0, 10) >5)
	{
		%this.owner.setLinearVelocityX(%speed);
		%this.owner.setFlipX(false);
	}
	else
	{
		%this.owner.setLinearVelocityX(-%speed);
		%this.owner.setFlipX(true);
	}
}

function MeanderBehavior::recycle(%this, %side)
{
	%speed = getRandom(%this.minSpeed, %this.maxSpeed);
	%this.owner.setLinearVelocityY*getRandom(-3, 3));
	%this.owner.setPositionY(getRandom(-15, 15));
	
	if (%side $= "left")
	{
		%this.owner.setLinearVelocityX(%speed);
		%this.owner.setFlipX(false);
	}
	else if (%side $= "right")
	{
		%this.owner.setLinearVelocityX(-%speed);
		%this.owner.setFlipX(true);
	}
}