# **3D Modeling Application - README**

## **Project Overview**  
This 3D Modeling Application is a versatile tool designed for creating, manipulating, and analyzing geometric models in a 3D space. With its robust features and intuitive interface, the application is suitable for both educational and professional use, enabling users to explore geometry, engineering designs, and mathematical principles dynamically.  

The application integrates tools for creating basic and advanced entities, applying constraints, managing transformations, and debugging. It is ideal for educators teaching geometry, CAD enthusiasts, and engineers working on conceptual designs.

---

## **Key Features**  
### **Constraints**  
- **Equal Constraints**: Ensure two lines have the same length.  
- **Perpendicular Constraints**: Enforce 90° angles between two entities.  
- **Tangent Constraints**: Make a line or curve touch another curve smoothly at a single point.  
- **Parallel Constraints**: Keep two lines equidistant without intersection.  
- **Angle Value Constraints**: Lock an angle between two entities to a specific value.  
- **Length Value Constraints**: Fix the length of a line or segment.  
- **Point On Constraints**: Constrain a point to lie on a specific line, curve, or surface.  
- **Dimension Value Constraints**: Restrict dimensions (e.g., height, width) to predefined values.  

### **Tools**  
- **New Tool**: Clears the workspace for fresh constructions.  
- **Load/Save Tools**: Import and export models to XML files.  
- **Move Tool**: Translate entities along a vector.  
- **Delete Tool**: Remove selected entities.  
- **Undo/Redo Tools**: Revert or reapply actions.  
- **Copy/Paste Tools**: Duplicate and reposition entities.  

### **Entity Creation**  
- **Point, Segment, Arc, and Circle Tools**: Create basic geometric shapes.  
- **Spline and Ellipse Tools**: Generate smooth, flexible curves and ellipses.  
- **Function-based Shape Tools**: Build parametric shapes using mathematical functions.  

### **Detail Management**  
- **Dimension Select**: Highlight and adjust specific dimensions.  
- **Detail Editor**: Central interface for managing tools, constraints, and entities.  

### **Debugging and Help**  
- **Debug Detail**: Inspect internal states, constraint satisfaction, and equation-solving progress.  
- **Tools Description**: Learn about tool usage with integrated tutorials and documentation.  

---

## **Installation**  
1. Clone this repository:  
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```  
2. Open the project in Unity (2020.3 or later is recommended).  
3. Install any necessary dependencies via the Unity Package Manager.  
4. Build and run the application.

---

## **Usage**  
### **Creating Entities**  
1. Open the **ToolBar** to select a creation tool (Point, Line, Circle, etc.).  
2. Click in the workspace to define the necessary points or parameters.  
3. Use **Dimension Select** to adjust or inspect properties like length, radius, or angles.  

### **Applying Constraints**  
1. Select two or more entities in the workspace.  
2. Open the **Constraints Panel** and apply a relevant constraint (e.g., Equal, Parallel).  
3. The system will solve and enforce the constraint immediately.  

### **Manipulating Entities**  
- Use **Move** or **Delete Tools** to reposition or remove entities.  
- Use **Undo/Redo** to explore design alternatives.  

### **Saving and Loading**  
- Save models via **File > Save** to export the workspace as an XML file.  
- Load models via **File > Load** to re-import saved files.

---

## **Educational Applications**  
Teachers can use the application to:  
- Demonstrate geometric principles like tangency, parallelism, and dimensional constraints.  
- Provide students with pre-saved models for hands-on exploration.  
- Illustrate advanced topics like parametric design or splines in engineering contexts.  

---

## **Contributing**  
We welcome contributions to enhance the functionality of this 3D Modeling Application. To contribute:  
1. Fork this repository.  
2. Create a new branch for your feature or bug fix.  
3. Submit a pull request with a detailed description of your changes.

---

## **Known Issues**  
- Performance may decrease with extremely complex models.  
- Some advanced constraints may fail in edge cases (e.g., conflicting tangents).  

--- 

## **Acknowledgments**  
Special thanks to contributors, testers, and the geometry/CAD community for their support and feedback.  

---

Feel free to customize further based on your project specifics!
