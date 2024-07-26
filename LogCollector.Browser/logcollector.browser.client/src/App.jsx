import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import LogsView from './components/LogsView';
import './App.css';

const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path="/logs" element={<LogsView />} />
                <Route path="/" element={<LogsView />} />
            </Routes>
        </Layout>
    );
};

export default App;
