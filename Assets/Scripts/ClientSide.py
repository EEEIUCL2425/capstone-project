import asyncio
import websockets
import json
import time

async def SendCommand(command, uri):
    async with websockets.connect(uri) as websocket:
        await websocket.send(json.dumps(command))

async def FetchData(uri):
    async with websockets.connect(uri) as websocket:
        response = await websocket.recv()
        return json.loads(response)

def RotateCamera(pitch, yaw, roll, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand({
        "command": "RotateCamera",
        "rotation": {"x": pitch, "y": yaw, "z": roll}
    }, uri))
    print("Camera rotated by", pitch, yaw, roll)

def MoveCamera(x, y, z, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "MoveCamera",
        "position": {"x": x, "y": y, "z": z}
    }, uri))
    print("Camera moved by", x, y, z)
    
#TODO: Add other functions

def GetCameraRotation(uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "GetCameraRotation"
    }, uri))

def RotateGripper(pitch, yaw, roll, GripperId, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand({
        "command": "RotateGripper",
        "rotation": {"x": pitch, "y": yaw, "z": roll},
        "gripperId": GripperId
    }, uri))
    print("Gripper rotated by", pitch, yaw, roll)

def MoveGripper(x, y, z, GripperId, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "MoveGripper",
        "position": {"x": x, "y": y, "z": z},
        "gripperId": GripperId
    }, uri))
    print("Gripper moved by", x, y, z)

def ResetGripper(GripperId, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "ResetGripper",
        "gripperId": GripperId
    }, uri))
    print("Gripper reset")

def Grip(GripperId, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "Grip",
        "gripperId": GripperId
    }, uri))
    print("Grip executed")

def Release(GripperId, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "Release",
        "gripperId": GripperId
    }, uri))
    print("Release executed")

def SpawnShelf(ShelfId, x, z, length, width, height, thickness, level, divider, uri="ws://localhost:8080/commands"):
    '''length, width, height determine the size of the space inside shelf
    thickness determines the thickness of each panel'''
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "SpawnShelf",
        "shelf_id": ShelfId,
        "x": x,
        "z": z,
        "length": length,
        "width": width,
        "height": height,
        "thickness": thickness,
        "level": level,
        "divider": divider
    }, uri))
    print("Shelf spawned. ID: " + ShelfId)

def BuildPresetStore(preset, level, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "BuildPresetStore",
        "preset": preset,
        "level": level
    }, uri))
    print("Preset store built.")

def BuildRandomStore(seed, StoreLength, StoreWidth, length, width, height, thickness, ShelfDistance, level, variation, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "BuildRandomStore",
        "seed": seed,
        "StoreLength": StoreLength,
        "StoreWidth" : StoreWidth,
        "length": length,
        "width": width,
        "height": height,
        "thickness": thickness,
        "ShelfDistance": ShelfDistance,
        "level": level,
        "variation": variation
        }, uri))
    print("Random store built.")

def FillShelves(seed, orientation, GroceryProducts, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "FillShelves",
        "seed": seed,
        "orientation": orientation,
        "GroceryProducts": GroceryProducts
    }, uri))
    print("Shelves filled.")

def SpawnRoom(length, width, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "SpawnRoom",
        "length": length,
        "width": width
    }, uri))
    print("Room spawned.")

def DestroyAllProducts(uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "DestroyAllProducts"
    }, uri))
    print("All products destroyed.")

def DestroyAllShelves(uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "DestroyAllShelves"
    }, uri))
    print("All shelves destroyed.")

def DestroyRoom(uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(SendCommand(
        {
        "command": "DestroyRoom"
    }, uri))
    print("Room destroyed.")



#grip("Right")
#move_gripper(-0.1, 0, 0, "Right")
#while(1):
#    rotate_gripper(0, 1, 0, "Right")
#release("Right")

'''def fluid_gripper_movement(gripper_id, uri="ws://localhost:8080/commands"):
    # Move gripper to initial position
    
    grip(gripper_id, uri)
    time.sleep(1)

    # Rotate gripper smoothly
    for angle in range(0, 360, 2):
        rotate_gripper(0, 4, 0, gripper_id, uri)
        time.sleep(0.02)
    
    # Move gripper in a smooth path
    for position in range(0, 40):
        move_gripper(0, 0.01, 0, gripper_id, uri)
        time.sleep(0.02)
    
    # Grip and release actions
    
    release(gripper_id, uri)
    time.sleep(1)'''

# Example usage
#fluid_gripper_movement("Right")