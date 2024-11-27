import axios from 'axios';
import variables from '../config/Variables';

const apiClient = axios.create({
    baseURL: variables.apiUrl,
});

const TaskService = {
    getTasks: () => apiClient.get('/GetTaskList'),
    getTask: (id) => apiClient.get(`/GetTask/${id}`),
    addTask: (task) => apiClient.post('/AddTask', task),
    updateTask: (task) => apiClient.put('/UpdateTask', task),
    deleteTask: (id) => apiClient.delete(`/DeleteTask/${id}`),
};

export default TaskService;
