import React, { useState,Fragment, useRef , useEffect} from 'react';
import ReactDOM from 'react-dom';
import './App.css';
import axios from 'axios';
function ListUser() {
  const apiUrl = "https://localhost:5001/faceapi";
  const [List, setList] = useState([]);
    function handleTakePhoto (dataUri) {
        // Do stuff with the photo...
      
        const user = {
          dataUri: dataUri,
          name : 'lorenzo'
        };
    
        axios.post(`https://localhost:5001/faceapi`, {  dataUri: dataUri,
        name : 'lorenzo' })
          .then(res => {
            console.log(res);
            console.log(res.data);
          })
      }
      useEffect(() => {
        console.log('calling use');
        const fetchData = async () => {
          const result = await axios(apiUrl);
          setList(result.data);
          
        };
      
        fetchData();
      }, []);
     
      return (
        <div>
       {List.map((item,index)=>{
              return  <div key={index}><img src={item.dataUri} ></img></div>
       })}
        </div>
      );
}

export default ListUser;
