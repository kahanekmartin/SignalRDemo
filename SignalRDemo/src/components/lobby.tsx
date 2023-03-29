import { useState, useEffect } from 'react'
import { Form, Button, InputGroup } from 'react-bootstrap'


interface Props
{
    logInUrl: string
}

export const Lobby = (props: Props) => {

    useEffect(() => {
        console.log('Loaded')
    }, []);

    const [user, setUser] = useState('');

    const logIn = (e: React.FormEvent) =>
    {
        console.log(user)

        e.preventDefault()

        fetch(props.logInUrl, {
            method: 'POST',
            redirect: 'follow',
            headers: {
              Accept: 'application.json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({ 'name': user })
          })
          .then(response => {
            console.log(response)
            if(response.redirected)
            {
                window.location.href = response.url
            }
          })
    }

    return <Form className='lobby message-form'
        onSubmit={logIn}>
        <Form.Group className='input'>
            <Form.Control placeholder="name" onChange={e => {
                console.log(user)
                setUser(e.target.value)} 
            }/>
        </Form.Group>
        <InputGroup>
            <Button variant="success" type="submit">Join</Button>
        </InputGroup>
    </Form>
}