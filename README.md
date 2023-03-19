# 3D-Augmented-Reality
This repository contains my final project for the 3D Augmented Reality course at the University of Padova for the school year 22/23. 

# Point Cloud Registration Final Project
This project consisted of three main parts:

## 1) Create a 3D scene in Unity and implement LiDAR
First, I used the Unity game engine to create a scene with a variety of different 3D objects that were downloaded from the Unity asset store. Then, I implemented a Light Detection and Ranging (LiDAR) script on a character capsule and recorded the spatial coordinates of the points to an output directory. 

## 2) Point Cloud Registration using ICP
After obtaining the output point clouds from my Unity scene, each composed of 10,000 points, I used Open3D's Iterative Closest Point (ICP) algorithm to iteratively map and transform each point cloud on to one another, creating a final merged point cloud that could then be used for visualization. 

## 3) Point Cloud Visualization
Finally, using the processed and merged final point cloud of about 2 million points, I used Open3D's visualization tools to display the final scene recreation.  
