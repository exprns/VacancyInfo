import React, { useState } from 'react';
import { TextField, Button } from '@material-ui/core'

export const Home = () => {
    const [text, setText] = useState('');

    const getFoo = () => {
        fetch(`api/Vacancy/Vacancies?name=${text}`).then(res => console.log(res)).catch(err => console.error(err))
    }

    const handleText = (e) => setText(e.target.value)

    return (
        <div className='row'>
            <TextField label="Поле для Масима" onChange={ handleText } />
            <Button
                color="primary"
                onClick={ getFoo }
            >
                Кнопочка
            </Button>
        </div>
    );
}
