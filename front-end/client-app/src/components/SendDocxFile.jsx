import './style.css'
import React, { useState } from 'react';

function FileUploadForm() {
  const [email, setEmail] = useState('');
  const [file, setFile] = useState(null);

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const formData = new FormData();
    formData.append('file', file);
    formData.append('email', email);

    const response = await fetch('https://localhost:7095/api/Upload', {
      method: 'POST',
      body: formData
    });

    if(response.ok === true){
      console.log("Accesfull!");
    }
    else{
      console.log("Error!");
    }
  };
  return (
    <div className="container">
      <h1 className='form__title'>SendFile</h1>
      <form className='form' onSubmit={handleSubmit}>
      <input className='input-email' placeholder='E-mail' pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$" type="text" onChange={handleEmailChange} required/>
        <label>
          <span>Pin File</span>
          <input className='input-file' onChange={handleFileChange} accept=".docx" type="file" required/>
        </label>
        <button className='button' type="submit">Submit</button>
      </form>
    </div>
  );
}

export default FileUploadForm;