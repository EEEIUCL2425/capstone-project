import asyncio
import websockets
import json
import time

async def send_command(command, uri):
    async with websockets.connect(uri) as websocket:
        await websocket.send(json.dumps(command))

async def fetch_data(uri):
    async with websockets.connect(uri) as websocket:
        response = await websocket.recv()
        return json.loads(response)

def rotate_camera(pitch, yaw, roll, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command({
        "command": "rotate_camera",
        "rotation": {"x": pitch, "y": yaw, "z": roll}
    }, uri))
    print("Camera rotated by", pitch, yaw, roll)

def move_camera(x, y, z, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command(
        {
        "command": "move_camera",
        "position": {"x": x, "y": y, "z": z}
    }, uri))
    print("Camera moved by", x, y, z)
    
#TODO: Add other functions

def get_camera_rotation(uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command(
        {
        "command": "get_camera_rotation"
    }, uri))

def rotate_gripper(pitch, yaw, roll, gripper_id, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command({
        "command": "rotate_gripper",
        "rotation": {"x": pitch, "y": yaw, "z": roll},
        "gripperId": gripper_id
    }, uri))
    print("Gripper rotated by", pitch, yaw, roll)

def move_gripper(x, y, z, gripper_id, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command(
        {
        "command": "move_gripper",
        "position": {"x": x, "y": y, "z": z},
        "gripperId": gripper_id
    }, uri))
    print("Gripper moved by", x, y, z)

def grip(gripper_id, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command(
        {
        "command": "grip",
        "gripperId": gripper_id
    }, uri))
    print("Grip executed")

def release(gripper_id, uri="ws://localhost:8080/commands"):
    asyncio.get_event_loop().run_until_complete(send_command(
        {
        "command": "release",
        "gripperId": gripper_id
    }, uri))
    print("Release executed")

#grip("Right")
#move_gripper(-0.1, 0, 0, "Right")
#while(1):
#    rotate_gripper(0, 1, 0, "Right")
#release("Right")

def fluid_gripper_movement(gripper_id, uri="ws://localhost:8080/commands"):
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
    time.sleep(1)

# Example usage
fluid_gripper_movement("Right")