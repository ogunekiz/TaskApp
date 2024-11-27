import React, { useState, useEffect } from 'react';
import TaskService from '../services/TaskService';
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";

import "primereact/resources/themes/lara-light-indigo/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";

import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

const MySwal = withReactContent(Swal);


function TaskComponent() {

    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);

    const [newTask, setNewTask] = useState({ title: "", description: "" });
    const [selectedTask, setSelectedTask] = useState(null);

    const [isUpdateDialogVisible, setIsUpdateDialogVisible] = useState(false);
    const [isCreateDialogVisible, setIsCreateDialogVisible] = useState(false);

    useEffect(() => {
        getTasks();
    }, []);


    // Get task list method
    const getTasks = () => {
        TaskService.getTasks()
            .then(response => {
                setTasks(response.data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Error fetching data: ', error);
            });
    };


    // Click add button
    const onCreate = () => {
        setIsCreateDialogVisible(true);
    };


    // Add task method
    const addTask = () => {
        if (newTask.title.trim() === "" || newTask.description === "") {
            toast.error("Please fill in the blank fields!", {
                position: "top-right",
                autoClose: 3000,
            });
            return;
        }

        TaskService.addTask(newTask)
            .then(() => {
                toast.success("Task added successfully", {
                    position: "top-right",
                    autoClose: 3000,
                });

                setNewTask({ title: "", description: "" });
                setIsCreateDialogVisible(false);
                getTasks();
            })
            .catch((error) => {
                console.error("An error occurred while adding the task: ", error);
                toast.error("An error occurred while adding the task!", {
                    position: "top-right",
                    autoClose: 3000,
                });
            });
    };


    // Click update button
    const onUpdate = (task) => {
        setSelectedTask({ ...task });
        setIsUpdateDialogVisible(true);
    };


    // Update task method
    const updateTask = async () => {
        if (selectedTask) {
            try {
                TaskService.updateTask(selectedTask);
                setTasks((prevTasks) =>
                    prevTasks.map((task) =>
                        task.id === selectedTask.id ? selectedTask : task
                    )
                );
                setIsUpdateDialogVisible(false);
                toast.success("The update process was completed successfully.", {
                    position: "top-right",
                    autoClose: 3000,
                });
            } catch (error) {
                console.error("An error occurred while updating the task:", error);
                toast.error("An error occurred while updating the task!", {
                    position: "top-right",
                    autoClose: 3000,
                });
            }
        }
    };


    // Delete task method
    const deleteTask = async (taskId) => {
        try {
            await TaskService.deleteTask(taskId);
            getTasks();
            MySwal.fire({
                title: "Success!",
                text: "Task deleted successfully!",
                icon: "success",
                confirmButtonText: "Ok",
                confirmButtonColor: "#4caf50",
            });
        } catch (error) {
            console.error("An error occurred while deleting the task: ", error);
            MySwal.fire({
                title: "Error!",
                text: "An error occurred while deleting the task!",
                icon: "error",
                confirmButtonText: "Ok",
                confirmButtonColor: "#f44336",
            });
        }
    };


    // Confirm method
    const confirmDelete = (taskId, taskTitle) => {
        MySwal.fire({
            title: "Confirmation of Delete",
            text: `Are you sure you want to delete the task ${taskTitle}?`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            confirmButtonColor: "#4caf50",
            cancelButtonColor: "#f44336",
        }).then((result) => {
            if (result.isConfirmed) {
                deleteTask(taskId);
            } else {
                console.log("Delete operation cancelled");
            }
        });
    };


    // Opreations Column Content
    const opreationsColumnContent = (rowData) => {
        return (
            <div className='button-group'>
                <Button
                    label="Update"
                    className="p-button-sm p-button-success"
                    icon="pi pi-pencil"
                    onClick={() => onUpdate(rowData)}
                />
                <Button
                    label="Delete"
                    className="p-button p-button-sm p-button-danger"
                    icon="pi pi-eraser"
                    onClick={() => confirmDelete(rowData.id, rowData.title)}
                    style={{
                        padding: "10px 20px",
                        background: "red",
                        color: "white",
                        border: "none",
                        borderRadius: "4px",
                        cursor: "pointer",
                    }}
                />
            </div>
        );
    };



    return (
        <div className="card">
            <ToastContainer />

            {/* Add Task Button */}
            <div style={{ marginBottom: '2rem', textAlign: 'left' }}>
                <Button
                    label="Add Task"
                    icon="pi pi-plus"
                    className="p-button p-button-info"
                    onClick={onCreate}
                />
            </div>

            {/* Task Table */}
            <DataTable
                value={tasks}
                showGridlines tableStyle={{ minWidth: '50rem' }}
                paginator
                rows={5}
                loading={loading}
                sortMode="single"
                removableSort
                responsiveLayout="scroll"
            >
                {/* <Column field="id" header="ID" style={{ textAlign: "center", width: "10%" }} sortable></Column> */}
                <Column field="title" header="Title" style={{ width: '40%' }} sortable filter></Column>
                <Column field="description" header="Description" style={{ width: '40%' }} sortable filter></Column>
                <Column
                    header="Operations"
                    alignHeader={'center'}
                    body={opreationsColumnContent}
                    style={{ textAlign: "center", width: "20%" }}
                />
            </DataTable>

            {/* Update Popup */}
            <Dialog
                visible={isUpdateDialogVisible}
                style={{ width: "600px" }}
                header="Update Task"
                modal
                draggable={false}
                onHide={() => setIsUpdateDialogVisible(false)}
            >
                <div className="p-fluid" style={{ gap: "2rem", display: "flex", flexDirection: "column" }}>
                    <div className="p-field" style={{ gap: "0.5rem", display: "flex", flexDirection: "column" }}>
                        <label htmlFor="title">Title: </label>
                        <InputText
                            id="title"
                            value={selectedTask?.title || ""}
                            onChange={(e) =>
                                setSelectedTask({ ...selectedTask, title: e.target.value })
                            }
                        />
                    </div>
                    <div className="p-field" style={{ gap: "0.5rem", display: "flex", flexDirection: "column" }}>
                        <label htmlFor="description">Description: </label>
                        <InputText
                            id="description"
                            value={selectedTask?.description || ""}
                            onChange={(e) =>
                                setSelectedTask({
                                    ...selectedTask,
                                    description: e.target.value,
                                })
                            }
                        />
                    </div>
                </div>
                <div className="p-dialog-footer" style={{ marginTop: '2rem' }}>
                    <Button
                        label="Cancel"
                        icon="pi pi-times"
                        className="p-button p-button-danger"
                        onClick={() => setIsUpdateDialogVisible(false)}
                    />
                    <Button
                        label="Save"
                        icon="pi pi-check"
                        className="p-button p-button-success"
                        onClick={updateTask}
                    />
                </div>
            </Dialog>

            {/* CreateDialog */}
            <Dialog
                visible={isCreateDialogVisible}
                style={{ width: "600px" }}
                header="Add Task"
                modal
                draggable={false}
                onHide={() => setIsCreateDialogVisible(false)}
            >
                <div
                    className="p-fluid"
                    style={{ gap: "2rem", display: "flex", flexDirection: "column" }}
                >
                    <div className="p-field" style={{ gap: "0.5rem", display: "flex", flexDirection: "column" }}>
                        <label htmlFor="title">Title: </label>
                        <InputText
                            id="title"
                            value={newTask.title}
                            onChange={(e) =>
                                setNewTask({ ...newTask, title: e.target.value })
                            }
                        // placeholder="Please enter task title"
                        />
                    </div>
                    <div className="p-field" style={{ gap: "0.5rem", display: "flex", flexDirection: "column" }}>
                        <label htmlFor="description">Description: </label>
                        <InputText
                            id="description"
                            value={newTask.description}
                            onChange={(e) =>
                                setNewTask({
                                    ...newTask,
                                    description: e.target.value,
                                })
                            }
                        // placeholder="Please enter task description"
                        />
                    </div>
                </div>

                <div className="p-dialog-footer" style={{ marginTop: '2rem' }}
                >
                    <Button
                        label="Cancel"
                        icon="pi pi-times"
                        className="p-button-sm p-button-danger"
                        onClick={() => setIsCreateDialogVisible(false)}
                    />
                    <Button
                        label="Save"
                        icon="pi pi-check"
                        className="p-button-sm p-button-success"
                        onClick={addTask}
                    />

                </div>
            </Dialog>
        </div>
    );

}

export default TaskComponent;